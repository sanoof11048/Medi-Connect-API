using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.PaymentDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models.Other;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly IGenericRepository<NursePayment> _genrRepo;
        private readonly IPaymentRepository _paymentRepo;
        private readonly INurseRequestRepository _nurseRequestRepository;
        private readonly IRazorpayService _razorpayService;
        public PaymentService(IGenericRepository<NursePayment> generic, IPaymentRepository paymentRepository, INurseRequestRepository nurseRequestRepository, IRazorpayService razorpayService, ILogger<PaymentService> logger)
        {
            _paymentRepo = paymentRepository;
            _genrRepo = generic;
            _nurseRequestRepository = nurseRequestRepository;
            _razorpayService = razorpayService;
            _logger = logger;
        }
        public async Task<ApiResponse<List<PendingPaymentDTO>>> GetMyPayments(Guid relativeId)
        {
            var assignments = await _nurseRequestRepository.GetAssignmentsByRelativeId(relativeId);

            var result = assignments.Select(a => new PendingPaymentDTO
            {
                AssignmentId = a.Id,
                NurseName = a.Nurse?.User?.FullName ?? "N/A",
                PatientName = a.Patient?.FullName ?? "N/A",
                TotalAmount = a.PaymentAmount,
                PaidAmount = a.TotalPaidToAdmin,
                PaymentStatus = a.PaymentToAdmin,
                AssignedDate = a.CreatedAt
            }).ToList();

            return new ApiResponse<List<PendingPaymentDTO>>(200, "Payments fetched", result);
        }

        public async Task<ApiResponse<string>> RecordPaymentAfterVerification(VerifyPaymentDTO dto, Guid relativeId)
        {
            var assignment = await _nurseRequestRepository.GetByIdAssignment(dto.AssignmentId);
            if (assignment == null || assignment.Patient?.RelativeId != relativeId)
                return new ApiResponse<string>(403, "Unauthorized or Invalid Assignment");

            if (assignment.TotalPaidToAdmin + dto.Amount > assignment.PaymentAmount)
                return new ApiResponse<string>(400, "Exceeds due amount");

            await _genrRepo.AddAsync(new NursePayment
            {
                Id = Guid.NewGuid(),
                NurseAssignmentId = dto.AssignmentId,
                TransactionId = dto.RazorpayPaymentId,
                AmountPaid = dto.Amount,
                IsToAdmin = true,
                Mode = dto.Mode,
            });

            assignment.TotalPaidToAdmin += dto.Amount;
            assignment.PaymentToAdmin = assignment.TotalPaidToAdmin >= assignment.PaymentAmount ? PaymentStatus.Paid : PaymentStatus.PartiallyPaid;
            await _nurseRequestRepository.UpdateAssignment(assignment);

            return new ApiResponse<string>(200, "Payment recorded successfully");
        }

        public async Task<ApiResponse<object>> InitializeRazorpayPayment(PartialPaymentDTO dto, Guid relativeId)
        {
            var assignment = await _nurseRequestRepository.GetByIdAssignment(dto.AssignmentId);
            if (assignment == null )
                return new ApiResponse<object>(403, "Unauthorized or Invalid Assignment");


            if (dto.Amount <= 0 || assignment.TotalPaidToAdmin + dto.Amount > assignment.PaymentAmount)
                return new ApiResponse<object>(400, "Invalid payment amount");

            var razorpayResult = await _razorpayService.ProcessPayment(relativeId, dto.Amount);
            if (!razorpayResult.Success)
                return new ApiResponse<object>(400, razorpayResult.ErrorMessage ?? "Payment init failed", razorpayResult);

            Console.WriteLine("Result: " + (razorpayResult?.Success ?? false));

            var response = new
            {
                orderId = razorpayResult.TransactionId,
                razorpayKey = _razorpayService.GetKey(),
                currency = "INR"
            };

            return new ApiResponse<object>(200, "Razorpay order created", response);
        }

        public async Task<ApiResponse<string>> VerifyAndRecordPayment(VerifyPaymentDTO dto, Guid relativeId)
        {
            var assignment = await _nurseRequestRepository.GetByIdAssignment(dto.AssignmentId);
            if (assignment == null || assignment.Patient?.RelativeId != relativeId)
                return new ApiResponse<string>(403, "Unauthorized or Invalid Assignment");

            if (assignment.TotalPaidToAdmin + dto.Amount > assignment.PaymentAmount)
                return new ApiResponse<string>(400, "Exceeds due amount");

            var isValid = _razorpayService.VerifySignature(dto);
            if (!isValid)
                return new ApiResponse<string>(400, "Invalid Razorpay signature");

            var payment = new NursePayment
            {
                Id = Guid.NewGuid(),
                NurseAssignmentId = assignment.Id,
                TransactionId = dto.RazorpayPaymentId,
                AmountPaid = dto.Amount,
                IsToAdmin = true,
                PaidById = relativeId,
                Mode = dto.Mode
            };
            await _genrRepo.AddAsync(payment);

            assignment.TotalPaidToAdmin += dto.Amount;
            assignment.PaymentToAdmin = assignment.TotalPaidToAdmin >= assignment.PaymentAmount
                ? PaymentStatus.Paid
                : PaymentStatus.PartiallyPaid;

            await _nurseRequestRepository.UpdateAssignment(assignment);

            return new ApiResponse<string>(200, "Payment verified and recorded");
        }

    }
}
