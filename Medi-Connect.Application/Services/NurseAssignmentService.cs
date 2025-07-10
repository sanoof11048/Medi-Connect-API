using AutoMapper;
using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.NurseDTO;
using Medi_Connect.Domain.DTOs.PaymentDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models.Other;
using Microsoft.EntityFrameworkCore;

public class NurseAssignmentService : INurseAssignmentService
{
    private readonly INurseRequestRepository _repo;
    private readonly IGenericRepository<NurseRequest> _geneRepo;
    private readonly IMapper _mapper;
    private readonly ICareTypeRateRepository _careTypeRateRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPatientRepository _petientRepository;
    private readonly INurseRepository _nurseRepository;

    public NurseAssignmentService(
        INurseRequestRepository repo,
        IMapper mapper,
        IGenericRepository<NurseRequest> genericRepository,
        IPaymentRepository paymentRepository,
        ICareTypeRateRepository careTypeRateRepository,
        IPatientRepository patientRepository,
        INurseRepository nurseRepository)
    {
        _repo = repo;
        _mapper = mapper;
        _geneRepo = genericRepository;
        _paymentRepository = paymentRepository;
        _careTypeRateRepository = careTypeRateRepository;
        _petientRepository = patientRepository;
        _nurseRepository = nurseRepository;
    }

    public async Task<ApiResponse<NurseRequestResponseDTO>> SendRequestAsync(NurseRequestDTO dto, Guid userId)
    {
        try
        {
            var request = _mapper.Map<NurseRequest>(dto);
            request.RequestedById = userId;

            var result = await _repo.CreateAsync(request);
            if (result == null)
                return new ApiResponse<NurseRequestResponseDTO>(500, "Failed to create nurse request.");

            var response = _mapper.Map<NurseRequestResponseDTO>(result);
            return new ApiResponse<NurseRequestResponseDTO>(200, "Request Sent", response);
        }
        catch (Exception ex)
        {
            return new ApiResponse<NurseRequestResponseDTO>(500, ex.Message, null, "Something went wrong while sending the request.");
        }
    }

    public async Task<ApiResponse<string>> AssignNurseAsync(AssignNurseDTO dto)
    {
        try
        {
            var request = await _repo.GetByIdWithRelationsAsync(dto.RequestId);
            if (request == null || request.Status != RequestStatus.Pending)
                return new ApiResponse<string>(400, "Invalid or already processed request");

            var careRate = await _careTypeRateRepository.GetByTypeAsync(request.CareType);
            if (careRate == null)
                return new ApiResponse<string>(404, "Payment rate for selected care type not found");

            if (request.PatientId == null)
                return new ApiResponse<string>(400, "Patient information missing in request.");

            var assignment = new NurseAssignment
            {
                NurseId = dto.NurseId,
                PatientId = request.PatientId,
                RequestId = request.Id,
                StartDate = request.StartDate,
                DurationDays = request.DurationDays,
                Status = AssignmentStatus.Active,
                PaymentAmount = careRate.FixedPayment,
            };

            await _repo.AssignNurse(assignment);
            request.Status = RequestStatus.Approved;
            await _geneRepo.UpdateAsync(request);

            var patient = request.Patient ?? await _petientRepository.GetPatientById(request.PatientId);
            if (patient != null)
            {
                patient.HomeNurseId = dto.NurseId;
                patient.IsNeedNurse = false;
                await _petientRepository.UpdateAsync(patient);
            }

            var nurseUser = await _nurseRepository.GetByIdAsync(dto.NurseId);
            if (nurseUser != null)
            {
                nurseUser.IsAvailable = false;
                await _nurseRepository.UpdateAsync(nurseUser);
            }

            return new ApiResponse<string>(200, "Nurse assigned successfully");
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(500, ex.Message, "Failed to assign nurse");
        }
    }

    public async Task<ApiResponse<string>> MarkPaymentAsync(MarkPaymentDTO dto)
    {
        try
        {
            var assignment = await _repo.GetByIdAssignment(dto.AssignmentId);
            if (assignment == null)
                return new ApiResponse<string>(404, "Assignment not found");

            if (dto.IsToNurse)
                assignment.PaymentToNurse = PaymentStatus.Paid;
            else
                assignment.PaymentToAdmin = PaymentStatus.Paid;

            await _repo.UpdateAssignment(assignment);
            return new ApiResponse<string>(200, "Payment status updated");
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(500, ex.Message, "Failed to update payment status");
        }
    }

    public async Task<ApiResponse<List<NurseAssignmentResponseDTO>>> GetAllAssignmentsAsync()
    {
        try
        {
            var data = await _repo.GetAllAssignmentsAsync();
            var result = _mapper.Map<List<NurseAssignmentResponseDTO>>(data);

            return new ApiResponse<List<NurseAssignmentResponseDTO>>(200, "All Assignments Fetched", result);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<NurseAssignmentResponseDTO>>(500, ex.Message, null, "Failed to fetch assignments");
        }
    }

    public async Task<ApiResponse<string>> PayToAdminPartial(PartialPaymentDTO dto, Guid relativeId)
    {
        try
        {
            var assignment = await _repo.GetByIdAssignment(dto.AssignmentId);
            if (assignment == null)
                return new ApiResponse<string>(404, "Assignment not found");

            if (assignment.Patient?.RelativeId != relativeId)
                return new ApiResponse<string>(403, "Not authorized");

            if (dto.Amount <= 0)
                return new ApiResponse<string>(400, "Invalid amount");

            if (assignment.TotalPaidToAdmin + dto.Amount > assignment.PaymentAmount)
                return new ApiResponse<string>(400, "Exceeds total due");

            var payment = new NursePayment
            {
                Id = Guid.NewGuid(),
                NurseAssignmentId = dto.AssignmentId,
                AmountPaid = dto.Amount,
                PaidById = relativeId,
                Mode = dto.Mode
            };
            await _paymentRepository.AddAsync(payment);

            assignment.TotalPaidToAdmin += dto.Amount;
            if (assignment.TotalPaidToAdmin >= assignment.PaymentAmount)
                assignment.PaymentToAdmin = PaymentStatus.Paid;

            await _repo.UpdateAssignment(assignment);

            return new ApiResponse<string>(200, $"₹{dto.Amount} paid successfully.");
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>(500, ex.Message, "Partial payment failed");
        }
    }

    public async Task<ApiResponse<IEnumerable<NurseRequestResponseDTO>>> GetAllRequests()
    {
        try
        {
            var requests = await _repo.GetAllRequestsAsync();
            var result = requests.Select(r => new NurseRequestResponseDTO
            {
                Id = r.Id,
                PatientName = r.Patient.FullName,
                RequestedBy = r.RequestedBy.FullName,
                Requirements = r.Requirements,
                StartDate = r.StartDate,
                PatientAge = r.Patient.Age,
                MedicalCondition = r.Patient.PhysicalCondition,
                DurationDays = r.DurationDays,
                Status = r.Status.ToString(),
                CareType = r.CareType.ToString()
            }).ToList();

            return new ApiResponse<IEnumerable<NurseRequestResponseDTO>>(200, "All Requests Fetched", result);
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<NurseRequestResponseDTO>>(500, ex.Message, null, "Failed to fetch nurse requests");
        }
    }
}
