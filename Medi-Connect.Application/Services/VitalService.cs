using AutoMapper;
using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.PatientDTO.VitalsDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models.PatientDetails;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Services
{
    public class VitalService : IVitalService
    {
        private readonly IGenericRepository<Vital> _repository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public VitalService(IGenericRepository<Vital> repository, IMapper mapper, IPatientRepository patientRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _patientRepository = patientRepository;
        }

        public async Task<ApiResponse<VitalResponseDTO>> CreateVitalAsync(CreateVitalsDTO dto)
        {
            try
            {
                var vital = _mapper.Map<Vital>(dto);
                await _repository.AddAsync(vital);
                var result = _mapper.Map<VitalResponseDTO>(vital);

                return new ApiResponse<VitalResponseDTO>(200, "Vital created successfully", result);
            }
            catch (Exception ex)
            {
                return new ApiResponse<VitalResponseDTO>(500, "Error creating vital", null, ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<VitalResponseDTO>>> GetVitalsByPatientIdAsync(Guid patientId)
        {
            try
            {
                var vitals = await _patientRepository.GetVitalsByPatientIdAsync(patientId);
                var result = _mapper.Map<IEnumerable<VitalResponseDTO>>(vitals);

                return new ApiResponse<IEnumerable<VitalResponseDTO>>(200, "Vitals fetched successfully", result);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<VitalResponseDTO>>(500, "Error fetching vitals", null, ex.Message);
            }
        }

        public async Task<ApiResponse<VitalResponseDTO>> GetVitalByIdAsync(Guid id)
        {
            try
            {
                var vital = await _repository.GetByIdAsync(id);
                if (vital == null)
                    return new ApiResponse<VitalResponseDTO>(404, "Vital not found");

                var result = _mapper.Map<VitalResponseDTO>(vital);
                return new ApiResponse<VitalResponseDTO>(200, "Vital fetched successfully", result);
            }
            catch (Exception ex)
            {
                return new ApiResponse<VitalResponseDTO>(500, "Error fetching vital", null, ex.Message);
            }
        }

        public async Task<ApiResponse<VitalResponseDTO>> UpdateVitalAsync(VitalUpdateDTO dto)
        {
            try
            {
                var vital = await _repository.GetByIdAsync(dto.VitalId);
                if (vital == null)
                    return new ApiResponse<VitalResponseDTO>(404, "Vital not found");

                _mapper.Map(dto, vital);
                await _repository.UpdateAsync(vital);

                var result = _mapper.Map<VitalResponseDTO>(vital);
                return new ApiResponse<VitalResponseDTO>(200, "Vital updated successfully", result);
            }
            catch (Exception ex)
            {
                return new ApiResponse<VitalResponseDTO>(500, "Error updating vital", null, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteVitalAsync(Guid id)
        {
            try
            {
                var vital = await _repository.GetByIdAsync(id);
                if (vital == null)
                    return new ApiResponse<bool>(404, "Vital not found", false);

                await _repository.DeleteAsync(vital);
                return new ApiResponse<bool>(200, "Vital deleted successfully", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(500, "Error deleting vital", false, ex.Message);
            }
        }
    }
}
