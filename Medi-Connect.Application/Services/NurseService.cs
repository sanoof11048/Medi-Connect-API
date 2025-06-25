using AutoMapper;
using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.NurseDTO;
using Medi_Connect.Domain.DTOs.PatientDTO;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Services
{
    public class NurseService : INurseService
    {
        private readonly INurseRepository _repository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public NurseService(INurseRepository repository, IMapper mapper, IPatientRepository patientRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _patientRepository = patientRepository;
        }

        public async Task<ApiResponse<NurseProfileResponseDto>> GetNurseProfileAsync(Guid id)
        {
            var nurseProfile = await _repository.GetByIdAsync(id);
            if (nurseProfile == null)
                return new ApiResponse<NurseProfileResponseDto>(404, "Nurse profile not found");

            var dto = _mapper.Map<NurseProfileResponseDto>(nurseProfile);
            return new ApiResponse<NurseProfileResponseDto>(200, "Nurse profile fetched", dto);
        }

        public async Task<ApiResponse<IEnumerable<NurseProfileResponseDto>>> GetAllNurseProfilesAsync()
        {
            var nurseProfiles = await _repository.GetAllAsync();
            var dtoList = _mapper.Map<IEnumerable<NurseProfileResponseDto>>(nurseProfiles);
            return new ApiResponse<IEnumerable<NurseProfileResponseDto>>(200, "All nurse profiles fetched", dtoList);
        }

        public async Task<ApiResponse<NurseProfileResponseDto>> CreateNurseProfileAsync(NurseProfileCreateDTO dto)
        {
            if (dto.HomeNurseId == Guid.Empty || dto.HomeNurseId == null)
                dto.HomeNurseId = Guid.NewGuid();

            var user = await _repository.GetUserByIdAsync(dto.HomeNurseId);

            if (user == null)
            {
                var userByEmail = await _repository.UserExistWithEmail(dto.Email);
                if (userByEmail)
                    return new ApiResponse<NurseProfileResponseDto>(400, "A user with this email already exists");

                user = new User
                {
                    Id = dto.HomeNurseId,
                    FullName = dto.FullName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Role = UserRole.HomeNurse,
                    IsActive = true,
                };

                await _repository.CreateUserAsync(user);
            }
            else
            {
                if (!string.Equals(user.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
                {
                    return new ApiResponse<NurseProfileResponseDto>(400, "Email does not match the existing user's email");
                }
            }

            if (await _repository.NurseProfileExistsAsync(dto.HomeNurseId))
                return new ApiResponse<NurseProfileResponseDto>(400, "Nurse profile already exists for this user");



            var profile = _mapper.Map<NurseProfile>(dto);
            await _repository.AddAsync(profile);

            var result = _mapper.Map<NurseProfileResponseDto>(profile);
            return new ApiResponse<NurseProfileResponseDto>(201, "Nurse profile created", result);
        }


        public async Task<ApiResponse<NurseProfileResponseDto>> UpdateNurseProfileAsync(Guid id, UpdateNurseProfileDTO dto)
        {
            var nurseProfile = await _repository.GetByIdAsync(id);
            if (nurseProfile == null)
                return new ApiResponse<NurseProfileResponseDto>(404, "Nurse profile not found");

            _mapper.Map(dto, nurseProfile);
            nurseProfile.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(nurseProfile);
            var updated = _mapper.Map<NurseProfileResponseDto>(nurseProfile);
            return new ApiResponse<NurseProfileResponseDto>(200, "Nurse profile updated", updated);
        }

        public async Task<ApiResponse<string>> DeleteNurseProfileAsync(Guid id)
        {
            var nurseProfile = await _repository.GetByIdAsync(id);
            if (nurseProfile == null)
                return new ApiResponse<string>(404, "Nurse profile not found");

            await _repository.DeleteAsync(id);
            return new ApiResponse<string>(200, "Nurse profile deleted");
        }

        public async Task<ApiResponse<IEnumerable<PatientResponseDTO>>> GetPatientOfHomeNurse(Guid userId)
        {
            try
            {

                var patients = await _patientRepository.GetPatientsByRelativeId(userId);
                if (patients == null || !patients.Any())
                {
                    return new ApiResponse<IEnumerable<PatientResponseDTO>>(404, "No More Patients Found");
                }

                var patientDTOs = _mapper.Map<IEnumerable<PatientResponseDTO>>(patients);

                return new ApiResponse<IEnumerable<PatientResponseDTO>>(200, "Patients Fetched", patientDTOs);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<PatientResponseDTO>>(500, ex.Message, null, ex.ToString());
            }
        }
    }
}
