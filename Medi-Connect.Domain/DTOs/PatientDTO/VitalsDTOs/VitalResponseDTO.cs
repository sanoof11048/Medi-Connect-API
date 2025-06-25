using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.PatientDTO.VitalsDTOs
{
    public class VitalResponseDTO
    {
        public Guid VitalId { get; set; }
        public Guid PatientId { get; set; }

        public string? BloodPressure { get; set; }
        public int? BloodSugar { get; set; }
        public float? Temperature { get; set; }
        public int? Pulse { get; set; }
        public int? Oxygen { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
