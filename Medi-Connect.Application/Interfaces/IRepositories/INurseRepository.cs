using Medi_Connect.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.IRepositories
{
    public interface INurseRepository
    {
        Task<NurseProfile?> GetByIdAsync(Guid id);
        Task<IEnumerable<NurseProfile>> GetAllAsync();
        Task AddAsync(NurseProfile profile);
        Task UpdateAsync(NurseProfile profile);
        Task DeleteAsync(Guid id);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<bool> NurseProfileExistsAsync(Guid userId);
        Task CreateUserAsync(User user);
        Task<bool> UserExistWithEmail(string email);

    }
}
