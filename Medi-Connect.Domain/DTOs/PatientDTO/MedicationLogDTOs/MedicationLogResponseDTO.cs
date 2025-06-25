using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.PatientDTO.MedicationLogDTOs
{
    public class MedicationLogResponseDTO
    {
        public Guid MedicationId { get; set; }
        public Guid PatientId { get; set; }
        public string? Name { get; set; }
        public string? Dosage { get; set; }
        public string? Frequency { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
