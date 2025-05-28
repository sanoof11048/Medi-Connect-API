using Medi_Connect.Domain.Models;
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
    }
}
