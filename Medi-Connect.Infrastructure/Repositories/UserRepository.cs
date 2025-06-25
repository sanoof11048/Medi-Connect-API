using AutoMapper;
using Azure.Core;
using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Domain.DTOs.Auth.OTP;
using Medi_Connect.Domain.DTOs.PatientDTO;
using Medi_Connect.Domain.DTOs.PatientDTO.FoodLogDTOs;
using Medi_Connect.Domain.DTOs.PatientDTO.MedicationLogDTOs;
using Medi_Connect.Domain.DTOs.PatientDTO.VitalsDTOs;
using Medi_Connect.Domain.DTOs.ReportDTOs;
using Medi_Connect.Domain.DTOs.UserDTOs;
using Medi_Connect.Domain.Models;
using Medi_Connect.Domain.Models.PatientDetails;
using Medi_Connect.Domain.Models.Users;
using Medi_Connect.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public UserRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.Where(u => !u.IsDeleted).FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task AddtoOtpStore(string email, string otp)
        {
            InMemoryOtpStore.OtpDict[email] = new OtpStore
            {
                Otp = otp,
                Expiry = DateTime.UtcNow.AddMinutes(5),
            };
        }
        public async Task<string> VerifyOtp(string email, string otp)
        {
            if (!InMemoryOtpStore.OtpDict.TryGetValue(email, out var storedOtp))
                return "No OTP Found";

            if (storedOtp.Expiry < DateTime.UtcNow)
                return "OTP Expired";
            if (storedOtp.Otp != otp)
                return "Invalid OTP.";
            return "Otp Verified";
        }

        public async Task<PatientReportDTO> GeneratePatientReportAsync(PatientReportRequestDTO request)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(x => x.Id == request.PatientId);
            var vitals = await _context.Vitals
            .Where(v => v.PatientId == request.PatientId && v.CreatedAt >= request.FromDate && v.CreatedAt <= request.ToDate && !v.IsDeleted)
            .ToListAsync();
            var foodLogs = await _context.FoodLogs
            .Where(f => f.PatientId == request.PatientId && f.CreatedAt >= request.FromDate && f.CreatedAt <= request.ToDate && !f.IsDeleted)
            .ToListAsync();
            var meds = await _context.MedicationLogs
            .Where(m => m.PatientId == request.PatientId && m.CreatedAt >= request.FromDate && m.CreatedAt <= request.ToDate && !m.IsDeleted)
            .ToListAsync();

            return new PatientReportDTO
            {
                Patient = _mapper.Map<PatientResponseDTO>(patient),
                Vitals = _mapper.Map<List<VitalResponseDTO>>(vitals),
                FoodLogs = _mapper.Map<List<FoodLogResponseDTO>>(foodLogs),
                Medications = _mapper.Map<List<MedicationLogResponseDTO>>(meds)
            };
        }

        public async Task<IEnumerable<User>> GetAllRelativesAsync()
        {
            return await _context.Users
                .Where(u => !u.IsDeleted && u.Role == UserRole.Relative)
                .Include(u => u.PatientsAsRelative)
                    .ThenInclude(p => p.HomeNurse)
                .ToListAsync();
        }

    }
}
