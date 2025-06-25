using Medi_Connect.Domain.DTOs.PatientDTO.MedicationLogDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.ISerives
{
    public interface IMedicationLogService
    {
        Task<ApiResponse<MedicationLogResponseDTO>> AddMedicationLogAsync(MedicationLogCreateDTO dto);
        Task<ApiResponse<MedicationLogResponseDTO>> GetByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<MedicationLogResponseDTO>>> GetByPatientIdAsync(Guid patientId);
        Task<ApiResponse<MedicationLogResponseDTO>> UpdateAsync(MedicationLogUpdateDTO dto);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }

}
