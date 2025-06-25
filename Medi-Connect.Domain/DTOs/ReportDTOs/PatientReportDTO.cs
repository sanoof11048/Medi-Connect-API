using Medi_Connect.Domain.DTOs.PatientDTO;
using Medi_Connect.Domain.DTOs.PatientDTO.FoodLogDTOs;
using Medi_Connect.Domain.DTOs.PatientDTO.MedicationLogDTOs;
using Medi_Connect.Domain.DTOs.PatientDTO.VitalsDTOs;


namespace Medi_Connect.Domain.DTOs.ReportDTOs
{
    public class PatientReportDTO
    {
        public PatientResponseDTO? Patient { get; set; }
        public List<VitalResponseDTO>? Vitals { get; set; }
        public List<MedicationLogResponseDTO>? Medications { get; set; }
        public List<FoodLogResponseDTO>? FoodLogs { get; set; }
    }

}
