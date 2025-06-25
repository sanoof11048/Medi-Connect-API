using Medi_Connect.Domain.DTOs.PatientDTO;
using Medi_Connect.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.UserDTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Address { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<PatientResponseDTO>? PatientsAsRelative { get; set; }
        public ICollection<PatientResponseDTO>? PatientsAsHomeNurse { get; set; }

    }
}
