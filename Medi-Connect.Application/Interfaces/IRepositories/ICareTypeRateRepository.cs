using Medi_Connect.Domain.Models.Admin;
using Medi_Connect.Domain.Models.PatientDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.IRepositories
{
    public interface ICareTypeRateRepository
    {
        Task<IEnumerable<CareTypeRate>> GetAllAsync();
        Task<CareTypeRate?> GetByIdAsync(Guid id);
        Task<CareTypeRate?> GetByTypeAsync(CareServiceType type);
        Task AddAsync(CareTypeRate entity);
        Task UpdateAsync(CareTypeRate entity);
        Task DeleteAsync(Guid id);
    }

}
