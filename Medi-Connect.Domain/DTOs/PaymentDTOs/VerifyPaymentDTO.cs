using Medi_Connect.Domain.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.PaymentDTOs
{
    public class VerifyPaymentDTO
    {
        public Guid AssignmentId { get; set; }
        public string? RazorpayPaymentId { get; set; }
        public string? RazorpayOrderId { get; set; }
        public string? RazorpaySignature { get; set; }
        public decimal Amount { get; set; }
        public PaymentMode Mode { get; set; } = PaymentMode.Online;
    }

}
