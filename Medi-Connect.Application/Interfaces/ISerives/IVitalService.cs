using Medi_Connect.Domain.DTOs.PatientDTO.VitalsDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.ISerives
{
    public interface IVitalService
    {
        Task<ApiResponse<VitalResponseDTO>> CreateVitalAsync(CreateVitalsDTO dto);
        Task<ApiResponse<IEnumerable<VitalResponseDTO>>> GetVitalsByPatientIdAsync(Guid patientId);
        Task<ApiResponse<VitalResponseDTO>> GetVitalByIdAsync(Guid id);
        Task<ApiResponse<VitalResponseDTO>> UpdateVitalAsync(VitalUpdateDTO dto);
        Task<ApiResponse<bool>> DeleteVitalAsync(Guid id);
    }

}
