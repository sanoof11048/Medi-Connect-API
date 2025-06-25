using Medi_Connect.Domain.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.PaymentDTOs
{
    public class PendingPaymentDTO
    {
        public Guid AssignmentId { get; set; }
        public string? NurseName { get; set; }
        public string? PatientName { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? PendingAmount => TotalAmount - PaidAmount;
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime AssignedDate { get; set; }
    }
}
