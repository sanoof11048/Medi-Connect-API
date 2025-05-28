using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medi_Connect.Domain.Models
{
    public class RelativeProfile
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid RelativeId { get; set; }
        [Required]
        public Guid PatientId { get; set; }


        [Required]
        public DateTime LinkedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? Relationship { get; set; } 

        
        [ForeignKey("PatientId")]
        public virtual PatientProfile Patient { get; set; }

        [ForeignKey("RelativeId")]
        public virtual User User { get; set; }
    }
}
