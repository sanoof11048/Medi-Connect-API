using Medi_Connect.Domain.DTOs.PatientDTO.FoodLogDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.ISerives
{
    public interface IFoodLogService
    {
        Task<ApiResponse<FoodLogResponseDTO>> AddFoodLogAsync(FoodLogCreateDTO dto);
        Task<ApiResponse<FoodLogResponseDTO>> GetByIdAsync(Guid mealId);
        Task<ApiResponse<IEnumerable<FoodLogResponseDTO>>> GetByPatientIdAsync(Guid patientId);

        Task<ApiResponse<FoodLogResponseDTO>> UpdateAsync(FoodLogUpdateDTO dto);
        Task<ApiResponse<bool>> DeleteAsync(Guid mealId);
    }
}
