using Medi_Connect.Domain.DTOs.Auth.OTP;
using Medi_Connect.Domain.DTOs.ReportDTOs;
using Medi_Connect.Domain.Models;
using Medi_Connect.Domain.Models.PatientDetails;
using Medi_Connect.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
        Task AddtoOtpStore(string email, string otp);
        Task<string> VerifyOtp(string email, string otp);
        Task<PatientReportDTO> GeneratePatientReportAsync(PatientReportRequestDTO request);
        Task<IEnumerable<User>> GetAllRelativesAsync();
    }
}
