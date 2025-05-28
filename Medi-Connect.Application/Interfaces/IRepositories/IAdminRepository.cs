using Medi_Connect.Domain.DTOs.UserDTOs;
using Medi_Connect.Domain.Models;
using Medi_Connect.Domain.Models.ApiResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.IRepositories
{
    public interface IAdminRepository
    {
        Task<List<User>> GetAllNurses();
        Task<bool> AddNurseAsync(User nurseDTO);
        Task<User> GetUserByEmail(string email);

    }
}
