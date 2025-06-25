using Medi_Connect.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.Models.Users
{
    public class NurseProfile : BaseEntity
    {
        [Key]
        [ForeignKey("User")]
        public Guid HomeNurseId { get; set; }

        public string? Qualification { get; set; }
        public int ExperienceYears { get; set; }
        public string? Bio { get; set; }
        public DateOnly DOB { get; set; }
        public DateOnly AvailableOn { get; set; }
        public bool IsAvailable { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        public decimal? ExpectedPayment { get; set; }
        public User? User { get; set; } = null!;
    }

}
