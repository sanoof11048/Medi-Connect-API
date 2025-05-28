using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medi_Connect.Infrastructure.Context;
using Medi_Connect.Application.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Medi_Connect.Domain.DTOs.UserDTOs;

namespace Medi_Connect.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;
        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllNurses()
        {
            var nurses = await _context.Users
                .Where(n=> n.Role == UserRole.Nurse)
                .Include(a=>a.NurseProfile)
                .ToListAsync();

            return nurses;
        }

        public async Task<bool> AddNurseAsync(User nurseDTO)
        {
            await _context.Users.AddAsync(nurseDTO);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
