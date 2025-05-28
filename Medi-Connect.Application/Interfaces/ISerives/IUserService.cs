using Medi_Connect.Domain.DTOs.UserDTO;
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
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<ApiResponse<string>> ToggleBlockUser(Guid id);
        Task<ApiResponse<User>> GetById(Guid id);
        Task<ApiResponse<string>> DeleteUser(Guid id);
        //Task<NurseProfile> AddNurseProfileAsync(NurseProfileCreateDTO dto);
        //Task<RelativeProfile> AddRelativeProfileAsync(RelativeProfileCreateDTO dto);
    }
}
