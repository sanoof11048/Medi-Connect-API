using Medi_Connect.Domain.Models.PatientDetails;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.PatientDTO
{
    public class CreatePatientDTO
    {
        [Required]
        public string FullName { get; set; } = null!;

        [Range(0, 120)]
        public int Age { get; set; }


        [Required]
        public string Gender { get; set; } = null!;

        [Required]
        public DateOnly DOB { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PhysicalCondition PhysicalCondition { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PatientCareType CareType { get; set; }


        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CareServiceType ServiceType { get; set; }

        public string? MedicalHistory { get; set; }

        public IFormFile? PhotoFile { get; set; }
    }
}
