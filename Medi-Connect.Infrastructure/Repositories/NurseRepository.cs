using Medi_Connect.Application.Interfaces.IRepositories;
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
    public class NurseRepository : INurseRepository
    {
        private readonly AppDbContext _context;
        public NurseRepository(AppDbContext context) => _context = context;

        public async Task<NurseProfile?> GetByIdAsync(Guid id) =>
            await _context.HomeNurses.Include(np => np.User).FirstOrDefaultAsync(np => np.HomeNurseId == id);

        public async Task<IEnumerable<NurseProfile>> GetAllAsync() =>
            await _context.HomeNurses.Include(np => np.User).ToListAsync();

        public async Task AddAsync(NurseProfile profile)
        {
            await _context.HomeNurses.AddAsync(profile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NurseProfile profile)
        {
            _context.HomeNurses.Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var profile = await _context.HomeNurses.FindAsync(id);
            if (profile != null)
            {
                _context.HomeNurses.Remove(profile);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetUserByIdAsync(Guid id) =>
            await _context.Users.FindAsync(id);

        public async Task<bool> NurseProfileExistsAsync(Guid userId) =>
            await _context.HomeNurses.AnyAsync(n => n.HomeNurseId == userId);

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        
        public async Task<bool> UserExistWithEmail(string email)=>
            await _context.Users.AnyAsync(u => u.Email == email);
        
    }

}
