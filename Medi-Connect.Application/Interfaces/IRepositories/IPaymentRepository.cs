using Medi_Connect.Domain.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.IRepositories
{
    public interface IPaymentRepository
    {
        //Task<bool> ProcessPayment(Guid relativeId, decimal amount);
        Task AddAsync(NursePayment payment);
        Task UpdateAsync(NursePayment payment);
    }
}
