using Medi_Connect.Domain.DTOs.NurseDTO;
using Medi_Connect.Domain.DTOs.PatientDTO;
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
    public interface IAdminService
    {
        Task<ApiResponse<ICollection<Patient>>> GetAllPatients();
    }
}
