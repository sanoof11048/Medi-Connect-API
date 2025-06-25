using CloudinaryDotNet.Actions;
using Medi_Connect.Domain.Common;
using Medi_Connect.Domain.Models.PatientDetails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.Models.Users
{
    public class User : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string? FullName { get; set; }
        [Required, EmailAddress]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Address { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public NurseProfile? NurseProfile { get; set; }
        public ICollection<Patient>? PatientsAsRelative { get; set; } = new List<Patient>();
        public ICollection<Patient>? PatientsAsHomeNurse { get; set; } = new List<Patient>();


    }

    public enum UserRole
    {
        Admin,
        Relative,
        HomeNurse,
    }

}
