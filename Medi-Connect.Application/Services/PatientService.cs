using AutoMapper;
using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.PatientDTO;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models.PatientDetails;
using System;


namespace Medi_Connect.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository repository, IMapper mapper, ICloudinaryService cloudinary)
        {
            _repository = repository;
            _mapper = mapper;
            _cloudinaryService = cloudinary;
        }

        public async Task<ApiResponse<PatientResponseDTO>> GetByIdAsync(Guid id)
        {
            var patient = await _repository.GetPatientById(id);

            if (patient == null)
                return new ApiResponse<PatientResponseDTO>(404, "Patient not found");

            var patientDto = _mapper.Map<PatientResponseDTO>(patient);
            return new ApiResponse<PatientResponseDTO>(200, "Patient retrieved", patientDto);
        }

        public async Task<ApiResponse<IEnumerable<PatientResponseDTO>>> GetAllAsync()
        {
            var patients = await _repository.GetAllAsync();
            var patientDtos = _mapper.Map<List<PatientResponseDTO>>(patients);

            foreach (var patient in patients)
            {
                var dto = patientDtos.FirstOrDefault(p => p.Id == patient.Id);

            }

            return new ApiResponse<IEnumerable<PatientResponseDTO>>(200, "Patients retrieved", patientDtos);
        }

        public async Task<ApiResponse<PatientResponseDTO>> CreatePatientAsync(CreatePatientDTO patientDto, Guid relativeId)
        {
            try
            {
                var relative = await _repository.GetRelativeByIdAsync(relativeId);
                if (relative == null)
                    return new ApiResponse<PatientResponseDTO>(404, "Relative not found");

                var patient = _mapper.Map<Patient>(patientDto);
                patient.Id = Guid.NewGuid();

                if (patientDto.PhotoFile != null)
                {
                    var url = await _cloudinaryService.UploadImageAsync(patientDto.PhotoFile);
                    patient.PhotoUrl = url;
                }

                patient.RelativeId = relativeId;
                await _repository.AddAsync(patient);
                var dto = _mapper.Map<PatientResponseDTO>(patient);

                return new ApiResponse<PatientResponseDTO>(201, "Patient created", dto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PatientResponseDTO>(500, ex.Message, null, "Something went Wrong");
            }
        }


        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var existing = await _repository.GetPatientById(id);
                if (existing == null || existing.IsDeleted)
                    return new ApiResponse<bool>(404, "Patient not found", false);

                existing.IsDeleted = true;
                await _repository.UpdateAsync(existing);

                return new ApiResponse<bool>(200, "Patient deleted", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(500, $"An error occurred: {ex.Message}", false);
            }
        }

        public async Task<ApiResponse<PatientResponseDTO>> UpdatePatientAsync(UpdatePatientDTO updatedPatient, Guid relativeId)
        {
            var relative = await _repository.GetRelativeByIdAsync(relativeId);
            if (relative == null)
                return new ApiResponse<PatientResponseDTO>(404, "Relative not found");

            var existing = await _repository.GetPatientById(updatedPatient.Id);
            if (existing == null || existing.IsDeleted)
                return new ApiResponse<PatientResponseDTO>(404, "Patient not found");



            existing.FullName = updatedPatient.FullName;
            existing.Age = updatedPatient.Age;
            existing.Gender = updatedPatient.Gender;
            if (updatedPatient.PhotoFile != null)
            {
                var url = await _cloudinaryService.UploadImageAsync(updatedPatient.PhotoFile);
                existing.PhotoUrl = url;
            }
            await _repository.UpdateAsync(existing);
            var dto = _mapper.Map<PatientResponseDTO>(existing);
            return new ApiResponse<PatientResponseDTO>(200, "Patient updated", dto);
        }

        public async Task<ApiResponse<IEnumerable<PatientResponseDTO>>> GetReport(int fromAge, int toAge, CareServiceType servicetype, string name)
        {
            try
            {

                var patient = await _repository.GetPatientReports(fromAge,  toAge, servicetype, name);
                var patients = _mapper.Map<List<PatientResponseDTO>>(patient);
                return new ApiResponse<IEnumerable<PatientResponseDTO>>(200, " ", patients, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<PatientResponseDTO>>(500, ex.Message, null, "Something Went Wrong" );
            }
        } 
    }
}
