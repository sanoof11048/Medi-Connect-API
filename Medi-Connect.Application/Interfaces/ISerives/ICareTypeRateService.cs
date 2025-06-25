using Medi_Connect.Domain.DTOs.PaymentDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.ISerives
{
    public interface ICareTypeRateService
    {
        Task<ApiResponse<List<CareTypeRateResponseDTO>>> GetAllAsync();
        Task<ApiResponse<CareTypeRateResponseDTO?>> GetByIdAsync(Guid id);
        Task<ApiResponse<string>> CreateAsync(CareTypeRateCreateDTO dto);
        Task<ApiResponse<string>> UpdateAsync(CareTypeRateUpdateDTO dto);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}
