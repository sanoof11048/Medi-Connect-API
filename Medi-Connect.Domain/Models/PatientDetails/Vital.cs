using Medi_Connect.Domain.Common;
using Medi_Connect.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Medi_Connect.Domain.Models.PatientDetails
{
    public class Vital : BaseEntity
    {
        [Key]
        public Guid VitalId { get; set; }
        [Required]
        public Guid PatientId { get; set; }


        public string? BloodPressure { get; set; }
        public int? BloodSugar { get; set; }
        public float? Temperature { get; set; }
        public int? Pulse {  get; set; }
        public int? Oxygen { get; set; }

        [StringLength(200)]
        public string? Notes { get; set; }

        [ForeignKey("PatientId")]
        public Patient? Patient { get; set; }
    }
}
