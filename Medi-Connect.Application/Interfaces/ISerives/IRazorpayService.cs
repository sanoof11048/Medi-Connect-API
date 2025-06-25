using Medi_Connect.Domain.DTOs.PaymentDTOs;
using Medi_Connect.Domain.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.ISerives
{
    public interface IRazorpayService
    {
        Task<PaymentResult> ProcessPayment(Guid userId, decimal amount);
        bool VerifySignature(VerifyPaymentDTO dto);
        string GetKey();
    }
}
