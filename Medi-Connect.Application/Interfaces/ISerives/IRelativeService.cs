using Medi_Connect.Domain.DTOs.PatientDTO;
using Medi_Connect.Domain.DTOs.RelativeDTO;
using Medi_Connect.Domain.DTOs.UserDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models.PatientDetails;
using Medi_Connect.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.ISerives
{
    public interface IRelativeService
    {
        Task<ApiResponse<IEnumerable<PatientResponseDTO>>> GetPatientOfRelative(Guid userId);
        Task<ApiResponse<IEnumerable<UserDTO>>> GetAllRelatives();
        //Task<ApiResponse<string>> HireNurse(HireNurseDTO dto);

    }
}
