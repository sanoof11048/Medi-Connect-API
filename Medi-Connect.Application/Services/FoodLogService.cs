using AutoMapper;
using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.PatientDTO.FoodLogDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models.PatientDetails;

namespace Medi_Connect.Application.Services
{
    public class FoodLogService : IFoodLogService
    {
        private readonly IGenericRepository<FoodLog> _foodLogRepository;
        private readonly IMapper _mapper;
        private readonly IPatientRepository _patientRepository;
        public FoodLogService(IGenericRepository<FoodLog> genericRepository, IMapper mapper, IPatientRepository patientRepository)
        {
            _foodLogRepository = genericRepository;
            _mapper = mapper;
            _patientRepository = patientRepository;
        }

        public async Task<ApiResponse<FoodLogResponseDTO>> AddFoodLogAsync(FoodLogCreateDTO dto)
        {
            try
            {
                var patient = await _patientRepository.GetPatientById(dto.PatientId);
                if (patient == null)
                    return new ApiResponse<FoodLogResponseDTO>(400, "Patient Not Found",null);

                var foodLog = _mapper.Map<FoodLog>(dto);
                await _foodLogRepository.AddAsync(foodLog);
                var responseDto = _mapper.Map<FoodLogResponseDTO>(foodLog);

                return new ApiResponse<FoodLogResponseDTO>(200, "Food log added successfully.",responseDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<FoodLogResponseDTO>(500,"Failed to add food log: " + ex.Message);
            }
        }

        public async Task<ApiResponse<FoodLogResponseDTO>> GetByIdAsync(Guid mealId)
        {
            try
            {
                var foodlog = await _foodLogRepository.GetByIdAsync(mealId);

                if (foodlog == null)
                    return new ApiResponse<FoodLogResponseDTO>(404, "Not Found");

                var result = _mapper.Map<FoodLogResponseDTO>(foodlog);
                return new ApiResponse<FoodLogResponseDTO>(200, "FoodLog Fetched", result);
            }
            catch (Exception ex)
            {
                return new ApiResponse<FoodLogResponseDTO>(500, "Somthing Went Wrong", null, ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<FoodLogResponseDTO>>> GetByPatientIdAsync(Guid patientId)
        {
            try
            {
                var foodlogs = await _patientRepository.GetFoodLogsByPatientIdAsync(patientId);
                var result = _mapper.Map<IEnumerable<FoodLogResponseDTO>>(foodlogs);
                return new ApiResponse<IEnumerable<FoodLogResponseDTO>>(200, "FoodLogs Fetched", result);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<FoodLogResponseDTO>>(500, "Somthing Went Wrong", null, ex.Message);
            }
        }

        public async Task<ApiResponse<FoodLogResponseDTO>> UpdateAsync(FoodLogUpdateDTO dto)
        {
            try
            {
                var foodlog = await _foodLogRepository.GetByIdAsync(dto.MealId);

                if (foodlog == null)
                    return new ApiResponse<FoodLogResponseDTO>(404, "Not Found");

                _mapper.Map(dto, foodlog);
                await _foodLogRepository.UpdateAsync(foodlog);
                return new ApiResponse<FoodLogResponseDTO>(200,"Foddlog Updated");
            }
            catch(Exception ex)
            {
                return new ApiResponse<FoodLogResponseDTO>(500, ex.Message, null);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid mealId)
        {
            try
            {
                var foodlog = await _foodLogRepository.GetByIdAsync(mealId);

                if (foodlog == null)
                    return new ApiResponse<bool>(404, "Delete failed. Food log not found");

                await _foodLogRepository.DeleteAsync(foodlog);
                return new ApiResponse<bool>(200, "Deleted successfully", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(500, ex.Message);
            }
        }

    }
}
