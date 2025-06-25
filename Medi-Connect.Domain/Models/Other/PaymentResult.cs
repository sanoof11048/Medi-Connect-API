using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.Models.Other
{
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public decimal? Amount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ErrorMessage { get; set; }

    }
}
