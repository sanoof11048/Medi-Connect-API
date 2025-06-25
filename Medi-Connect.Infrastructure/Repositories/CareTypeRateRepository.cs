using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Domain.Models.Admin;
using Medi_Connect.Domain.Models.PatientDetails;
using Medi_Connect.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Medi_Connect.Infrastructure.Repositories
{
    public class CareTypeRateRepository : ICareTypeRateRepository
    {
        private readonly AppDbContext _context;

        public CareTypeRateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CareTypeRate>> GetAllAsync()
        {
            return await _context.CareTypeRates
                .Where(c => !c.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CareTypeRate?> GetByIdAsync(Guid id)
        {
            return await _context.CareTypeRates
                                 .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<CareTypeRate?> GetByTypeAsync(CareServiceType type)
        {
            return await _context.CareTypeRates
                                 .FirstOrDefaultAsync(x => x.ServiceType == type && !x.IsDeleted);
        }

        public async Task AddAsync(CareTypeRate entity)
        {
            await _context.CareTypeRates.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CareTypeRate entity)
        {
            _context.CareTypeRates.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.CareTypeRates.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

}
