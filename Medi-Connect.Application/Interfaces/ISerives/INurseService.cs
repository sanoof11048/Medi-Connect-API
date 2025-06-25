using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Domain.DTOs.NurseDTO;
using Medi_Connect.Domain.DTOs.PatientDTO;
using Medi_Connect.Domain.DTOs.UserDTOs;
using Medi_Connect.Domain.Models;
using Medi_Connect.Domain.Models.ApiResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.ISerives
{
    public interface INurseService
    {
        Task<ApiResponse<NurseProfileResponseDto>> GetNurseProfileAsync(Guid id);
        Task<ApiResponse<IEnumerable<NurseProfileResponseDto>>> GetAllNurseProfilesAsync();
        Task<ApiResponse<NurseProfileResponseDto>> CreateNurseProfileAsync(NurseProfileCreateDTO dto);
        Task<ApiResponse<NurseProfileResponseDto>> UpdateNurseProfileAsync(Guid id, UpdateNurseProfileDTO dto);
        Task<ApiResponse<string>> DeleteNurseProfileAsync(Guid id);
        Task<ApiResponse<IEnumerable<PatientResponseDTO>>> GetPatientOfHomeNurse(Guid userId);
    }
}
