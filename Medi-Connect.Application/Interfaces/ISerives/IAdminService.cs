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
    public interface IAdminService
    {
        Task<ApiResponse<List<User>>> GetAllNurses();
        Task<ApiResponse<User>> AddNurse(CreateNurseDTO nurseDTO);
        Task<ApiResponse<string>> DeleteNurse(Guid id);
    }
}
