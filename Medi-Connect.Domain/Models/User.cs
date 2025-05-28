using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string? FullName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? Photo_url { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public  NurseProfile? NurseProfile { get; set; }
        public ICollection<PatientProfile> Patients { get; set; }
        public RelativeProfile? PatientRelative { get; set; }
    }

    public enum UserRole
    {
        Admin,
        Relative,
        Nurse,
        User
    }

}
