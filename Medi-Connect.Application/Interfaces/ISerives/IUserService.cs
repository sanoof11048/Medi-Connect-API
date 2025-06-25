using Medi_Connect.Domain.DTOs.PatientDTO;
using Medi_Connect.Domain.DTOs.ReportDTOs;
using Medi_Connect.Domain.DTOs.UserDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.ISerives
{
    public interface IUserService
    {
        Task<ApiResponse<IEnumerable<UserDTO>>> GetAllAsync();
        Task<ApiResponse<UserDTO>> GetById(Guid id);
        Task<ApiResponse<UserDTO>> UpdateUserAsync(UpdateUserDTO user);
        Task<ApiResponse<string>> ToggleBlockUser(Guid id);
        Task<ApiResponse<string>> DeleteUser(Guid id);
        Task<PatientReportDTO> GeneratePatientReportAsync(PatientReportRequestDTO request);

    }
}
