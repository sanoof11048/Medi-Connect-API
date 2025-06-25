using Medi_Connect.Domain.Common;
using Medi_Connect.Domain.Models.PatientDetails;
using Medi_Connect.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.Models.Other
{
    public class NurseAssignment : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid NurseId { get; set; }
        public Guid RequestId { get; set; }
        public DateTime StartDate { get; set; }
        public int DurationDays { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PaymentAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalPaidToAdmin { get; set; } = 0;
        [NotMapped]
        public bool IsFullyPaid => TotalPaidToAdmin >= PaymentAmount;


        public AssignmentStatus Status { get; set; } = AssignmentStatus.Active;
        public PaymentStatus PaymentToAdmin { get; set; } = PaymentStatus.Unpaid;
        public PaymentStatus PaymentToNurse { get; set; } = PaymentStatus.Unpaid;
        [ForeignKey(nameof(PatientId))]
        public Patient? Patient { get; set; }
        public NurseProfile? Nurse { get; set; }
        public NurseRequest? Request { get; set; }
    }

}
