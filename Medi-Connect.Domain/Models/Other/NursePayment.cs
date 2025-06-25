using Medi_Connect.Domain.Common;
using Medi_Connect.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.Models.Other
{
    public class NursePayment : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid NurseAssignmentId { get; set; }
        public Guid PaidById { get; set; }
        public string? TransactionId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }

        public DateTime PaidAt { get; set; } = DateTime.UtcNow;
        public bool IsToAdmin { get; set; } = true;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMode Mode { get; set; } = PaymentMode.Online; 
        public NurseAssignment NurseAssignment { get; set; } = null!;
        [ForeignKey("PaidById")]
        public User PaidBy { get; set; } = null!;
    }

    public enum PaymentMode
    {
        Online,
        UPI,
        Card,
        Cash
    }
    public enum PaymentStatus
    {
        Unpaid,
        PartiallyPaid,
        Paid
    }

}
