using Medi_Connect.Application.Interfaces.IRepositories;
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
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;
        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync() =>
             await _context.Patients
                .Where(p => !p.IsDeleted)
                .Include(p => p.Relative)
                .Include(p => p.Vitals)
                .Include(p => p.Medications)
                .Include(p => p.Meals)
                .Include(p => p.Documents)
                .Include(p => p.NurseAssignment)
                    .ThenInclude(na => na.Nurse)
                        .ThenInclude(np => np.User)
                .Include(p => p.NurseAssignment)
                    .ThenInclude(na => na.Request)
                .ToListAsync();
        public async Task<Patient?> GetPatientById(Guid id) =>
            await _context.Patients
            .Include(p => p.Relative).Include(p => p.HomeNurse)
            .Include(p => p.Vitals).Include(p => p.Meals).Include(p => p.Medications)
            .FirstOrDefaultAsync(p => !p.IsDeleted && p.Id == id);


        public async Task AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }

        //public async Task<ICollection<Patient>> GetPatientsByRelativeId(Guid relativeId)
        //{
        //    var patients = await _context.Patients
        //    .Where(p => !p.IsDeleted && (p.RelativeId == relativeId || p.HomeNurseId == relativeId))
        //    .Include(p=>p.HomeNurse)
        //    .Include(a=>a.)
        //    .Include(v=>v.Vitals).Include(p => p.Medications).Include(p=>p.Medications).Include(m=>m.Meals)
        //    .ToListAsync();
        //    return patients;
        //}

        public async Task<ICollection<Patient>> GetPatientsByRelativeId(Guid relativeId)
        {
            var patients = await _context.Patients
                .Where(p => !p.IsDeleted && (p.RelativeId == relativeId || p.HomeNurseId == relativeId))
                .Include(p => p.Relative)
                .Include(p => p.Vitals)
                .Include(p => p.Medications)
                .Include(p => p.Meals)
                .Include(p => p.Documents)
                .Include(p => p.NurseAssignment)
                    .ThenInclude(na => na.Nurse)
                        .ThenInclude(np => np.User)
                .Include(p => p.NurseAssignment)
                    .ThenInclude(na => na.Request)
                .ToListAsync();

            return patients;
        }


        public async Task<User?> GetRelativeByIdAsync(Guid relativeId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == relativeId && u.Role == UserRole.Relative);
        }
        public async Task<IEnumerable<Vital>> GetVitalsByPatientIdAsync(Guid patientId)
        {
            return await _context.Vitals
                .Where(v => v.PatientId == patientId && !v.IsDeleted)
                .ToListAsync();
        }


        public async Task<IEnumerable<FoodLog>> GetFoodLogsByPatientIdAsync(Guid patientId)
        {
            return await _context.FoodLogs
                .Where(v => v.PatientId == patientId && !v.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicationLog>> GetMedicationLogsByPatientIdAsync(Guid patientId)
        {
            return await _context.MedicationLogs
                .Where(v => v.PatientId == patientId && !v.IsDeleted)
                .ToListAsync();
        }

    }
}
