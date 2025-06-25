using Medi_Connect.Domain.Common;
using Medi_Connect.Domain.DTOs.NurseDTO;
using Medi_Connect.Domain.Models;
using Medi_Connect.Domain.Models.PatientDetails;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Medi_Connect.Domain.DTOs.PatientDTO
{
    public class PatientResponseDTO
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public int Age { get; set; }
        public DateOnly DOB { get; set; }
        public string? Gender { get; set; }
        public string? PhotoUrl { get; set; }
        public string? CareType { get; set; }
        public string? ServiceType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PhysicalCondition PhysicalCondition { get; set; }

        [NotMapped]
        public string[] PhysicalConditionDisplay => PhysicalCondition.GetFlagsDisplayNames();


        public string? MedicalHistory { get; set; }
        public decimal? Payment { get; set; }
        public bool IsNeedNurse { get; set; }

        public Guid RelativeId { get; set; }
        public string? RelativeName { get; set; }

        public Guid? NurseId { get; set; }
        public string? NurseName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public NurseAssignmentResponseDTO? NurseAssignmentResponse { get; set; }

        public ICollection<Vital> Vitals { get; set; } = new List<Vital>();
        public ICollection<MedicationLog> Medications { get; set; } = new List<MedicationLog>();
        public ICollection<FoodLog> Meals { get; set; } = new List<FoodLog>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}
