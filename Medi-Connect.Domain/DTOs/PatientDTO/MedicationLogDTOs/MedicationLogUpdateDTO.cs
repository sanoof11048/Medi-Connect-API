using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.PatientDTO.MedicationLogDTOs
{
    public class MedicationLogUpdateDTO
    {
        public Guid MedicationId { get; set; }
        public string Name { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string? Notes { get; set; }
    }
}
