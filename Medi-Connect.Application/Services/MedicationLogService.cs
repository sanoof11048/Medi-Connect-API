using AutoMapper;
using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.PatientDTO.MedicationLogDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models.PatientDetails;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MedicationLogService : IMedicationLogService
{
    private readonly IGenericRepository<MedicationLog> _medicationRepo;
    private readonly IPatientRepository _patientRepo;
    private readonly IMapper _mapper;

    public MedicationLogService(IGenericRepository<MedicationLog> medicationRepo, IPatientRepository patientRepo, IMapper mapper)
    {
        _medicationRepo = medicationRepo;
        _patientRepo = patientRepo;
        _mapper = mapper;
    }

    public async Task<ApiResponse<MedicationLogResponseDTO>> AddMedicationLogAsync(MedicationLogCreateDTO dto)
    {
        try
        {
            var patient = await _patientRepo.GetPatientById(dto.PatientId);
            if (patient == null)
                return new ApiResponse<MedicationLogResponseDTO>(404, "Patient not found");

            var entity = _mapper.Map<MedicationLog>(dto);
            await _medicationRepo.AddAsync(entity);

            var result = _mapper.Map<MedicationLogResponseDTO>(entity);
            return new ApiResponse<MedicationLogResponseDTO>(200, "Medication log added successfully", result);
        }
        catch (Exception ex)
        {
            return new ApiResponse<MedicationLogResponseDTO>(500, "Error while adding medication log", null, ex.Message);
        }
    }

    public async Task<ApiResponse<MedicationLogResponseDTO>> GetByIdAsync(Guid id)
    {
        try
        {
            var entity = await _medicationRepo.GetByIdAsync(id);
            if (entity == null)
                return new ApiResponse<MedicationLogResponseDTO>(404, "Medication log not found");

            var result = _mapper.Map<MedicationLogResponseDTO>(entity);
            return new ApiResponse<MedicationLogResponseDTO>(200, "Fetched medication log", result);
        }
        catch (Exception ex)
        {
            return new ApiResponse<MedicationLogResponseDTO>(500, "Error while fetching medication log", null, ex.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<MedicationLogResponseDTO>>> GetByPatientIdAsync(Guid patientId)
    {
        try
        {
            var list = await _patientRepo.GetMedicationLogsByPatientIdAsync(patientId);
            var result = _mapper.Map<IEnumerable<MedicationLogResponseDTO>>(list);

            return new ApiResponse<IEnumerable<MedicationLogResponseDTO>>(200, "Fetched medication logs", result);
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<MedicationLogResponseDTO>>(500, "Error while fetching medication logs", null, ex.Message);
        }
    }

    public async Task<ApiResponse<MedicationLogResponseDTO>> UpdateAsync(MedicationLogUpdateDTO dto)
    {
        try
        {
            var entity = await _medicationRepo.GetByIdAsync(dto.MedicationId);
            if (entity == null)
                return new ApiResponse<MedicationLogResponseDTO>(404, "Medication log not found");

            _mapper.Map(dto, entity);
            await _medicationRepo.UpdateAsync(entity);

            var result = _mapper.Map<MedicationLogResponseDTO>(entity);
            return new ApiResponse<MedicationLogResponseDTO>(200, "Medication log updated successfully", result);
        }
        catch (Exception ex)
        {
            return new ApiResponse<MedicationLogResponseDTO>(500, "Error while updating medication log", null, ex.Message);
        }
    }

    public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
    {
        try
        {
            var entity = await _medicationRepo.GetByIdAsync(id);
            if (entity == null)
                return new ApiResponse<bool>(404, "Medication log not found", false);

            await _medicationRepo.DeleteAsync(entity);
            return new ApiResponse<bool>(200, "Medication log deleted successfully", true);
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>(500, "Error while deleting medication log", false, ex.Message);
        }
    }
}
