using Medi_Connect.Domain.DTOs.UserDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models.PatientDetails;
using Medi_Connect.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.IRepositories
{
    public interface IAdminRepository
    {
        Task<List<NurseProfile>> GetAllNurses();
        Task<bool> AddNurseAsync(NurseProfile nurseDTO);
        Task<User> GetUserByEmail(string email);
        Task<ICollection<Patient>> GetAllPatients();
    }
}
