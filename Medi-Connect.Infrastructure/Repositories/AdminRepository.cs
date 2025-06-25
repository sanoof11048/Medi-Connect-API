using Medi_Connect.Domain.Models.ApiResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medi_Connect.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Medi_Connect.Domain.DTOs.UserDTOs;
using Medi_Connect.Domain.Models.Users;
using Medi_Connect.Domain.Models.PatientDetails;
using Medi_Connect.Application.Interfaces.IRepositories;

namespace Medi_Connect.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;
        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<NurseProfile>> GetAllNurses() =>
            await _context.HomeNurses
                .Include(a => a.User).ThenInclude(b => b.PatientsAsHomeNurse)
                .ToListAsync();


        public async Task<bool> AddNurseAsync(NurseProfile nurseDTO)
        {
            await _context.HomeNurses.AddAsync(nurseDTO);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.Where(u => !u.IsDeleted).FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<ICollection<Patient>> GetAllPatients()
        {
            return await _context.Patients.Include(r=>r.Relative).Include(n=>n.HomeNurse).ThenInclude(hn=>hn.NurseProfile).ToListAsync();
        }
    }
}
