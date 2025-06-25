using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.NurseDTO
{
    public class NurseProfileCreateDTO
    {
        public Guid HomeNurseId { get; set; } = Guid.NewGuid();
        [Required]
        [StringLength(100)]
        public string? FullName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        [StringLength(100)]
        public string? Qualification { get; set; }

        [Range(0, 50)]
        public int ExperienceYears { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        [Required]
        public DateOnly DOB { get; set; }

        [Required]
        public DateOnly AvailableOn { get; set; }

        public bool IsAvailable { get; set; }

    }
}
