using Medi_Connect.Domain.Common;
using Medi_Connect.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.Models.PatientDetails
{
    public class MedicationLog : BaseEntity
    {
        [Key]
        public Guid MedicationId { get; set; }
        [Required]
        public Guid PatientId { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        public string? Dosage { get; set; }

        [Required]
        public string? Frequency { get; set; }

        [StringLength(200)]
        public string? Notes { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

    }
}
