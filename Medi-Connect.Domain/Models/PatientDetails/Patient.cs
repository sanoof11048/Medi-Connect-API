using Medi_Connect.Domain.Common;
using Medi_Connect.Domain.Models.Other;
using Medi_Connect.Domain.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace Medi_Connect.Domain.Models.PatientDetails
{
    public class Patient : BaseEntity
    {
        
        public Guid Id { get; set; }

        [Required]
        public string? FullName { get; set; }

        public int Age { get; set; }
        public string? Gender { get; set; }
        public DateOnly DOB { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PatientCareType? CareType { get; set; } 
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CareServiceType? ServiceType { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PhysicalCondition PhysicalCondition { get; set; }
        public string? PhotoUrl { get; set; }
        [StringLength(200)]
        public string? MedicalHistory { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Payment { get; set; }
        public bool IsNeedNurse { get; set; } = true;
        public Guid? RelativeId { get; set; }

        [ForeignKey("RelativeId")]
        public User? Relative { get; set; }

        public Guid? HomeNurseId { get; set; }

        [ForeignKey("HomeNurseId")]
        public User? HomeNurse { get; set; }
        public NurseAssignment? NurseAssignment { get; set; }
        public ICollection<Vital> Vitals { get; set; } = new List<Vital>();
        public ICollection<MedicationLog> Medications { get; set; } = new List<MedicationLog>();
        public ICollection<FoodLog> Meals { get; set; } = new List<FoodLog>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }



    public enum CareServiceType
    {

        [Display(Name = "Neurological Care")]
        NeurologicalCare,

        [Display(Name = "Post Operative Care")]
        PostOperativeCare,

        [Display(Name = "Cancer Care")]
        CancerCare,

        [Display(Name = "Physio Therapy")]
        PhysioTherapy,

        [Display(Name = "Elderly Medical Care")]
        ElderlyMedicalCare,

        [Display(Name = "Tracheostomy Care")]
        TracheostomyCare,

        [Display(Name = "Home Physio Therapy")]
        HomePhysioTherapy,

    }



    public enum PatientCareType
    {

        [Display(Name = "Short Term")]
        ShortTerm,

        [Display(Name = "Long Term")]
        LongTerm,

        [Display(Name = "Palliative Care")]
        PalliativeCare,

        [Display(Name = "Rehabilitation")]
        Rehabilitation,

        [Display(Name = "Post Surgical Care")]
        PostSurgicalCare,

        [Display(Name = "Chronic Illness Care")]
        ChronicIllnessCare,

        [Display(Name = "Emergency Care")]
        EmergencyCare
    }

    [Flags]
    public enum PhysicalCondition
    {
        [Display(Name = "On Bed")]
        OnBed = 1,

        [Display(Name = "Can Walk")]
        CanWalk = 2,

        [Display(Name = "Need Support to Walk")]
        NeedSupport = 4,

        [Display(Name = "Wheelchair User")]
        Wheelchair = 8,

        [Display(Name = "Partially Paralyzed")]
        Paralyzed = 16,

        [Display(Name = "Unconscious")]
        Unconscious = 32
    }


}
