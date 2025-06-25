using Medi_Connect.Domain.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.PaymentDTOs
{
    public class PaymentTransactionDTO
    {
        public Guid AssignmentId { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public string NurseName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public PaymentMode Mode { get; set; }
    }

}
