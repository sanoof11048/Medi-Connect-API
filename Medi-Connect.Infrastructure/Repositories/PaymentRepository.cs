using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Domain.Models.Other;
using Medi_Connect.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }
        //public async Task<bool> ProcessPayment(Guid relativeId, decimal amount)
        //{

        //}

        public async Task AddAsync(NursePayment payment)
        {
            _context.NursePayments.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NursePayment payment)
        {
            _context.NursePayments.Update(payment);
            await _context.SaveChangesAsync();
        }

    }
}
