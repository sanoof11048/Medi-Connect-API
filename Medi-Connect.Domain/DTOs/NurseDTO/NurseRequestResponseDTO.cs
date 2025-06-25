using Medi_Connect.Domain.Models.Other;
using Medi_Connect.Domain.Models.PatientDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.NurseDTO
{
    public class NurseRequestResponseDTO
    {
        public Guid Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string RequestedBy { get; set; } = string.Empty;
        public int PatientAge { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PhysicalCondition? MedicalCondition { get; set; }
        public string? Requirements { get; set; }
        public DateTime StartDate { get; set; }
        public int DurationDays { get; set; }
        public string Status { get; set; } = string.Empty;
        public string CareType { get; set; } = string.Empty;
    }

}
