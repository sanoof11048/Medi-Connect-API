using Microsoft.AspNetCore.Http;
using Medi_Connect.Domain.Models.PatientDetails;
using System;
using System.Text.Json.Serialization;

namespace Medi_Connect.Domain.DTOs.PatientDTO
{
    public class UpdatePatientDTO
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }

        public int Age { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PhysicalCondition PhysicalCondition { get; set; }

        public string? Gender { get; set; }

        public DateOnly? DOB { get; set; }

        public PatientCareType? CareType { get; set; }

        public CareServiceType? ServiceType { get; set; }

        public string? MedicalHistory { get; set; }

        public IFormFile? PhotoFile { get; set; }

        public bool? IsNeedNurse { get; set; }
    }
}
