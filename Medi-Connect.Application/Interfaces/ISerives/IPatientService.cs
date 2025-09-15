using Medi_Connect.Domain.DTOs.PatientDTO;
using Medi_Connect.Domain.DTOs.ReportDTOs;
using Medi_Connect.Domain.Models;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models.PatientDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.ISerives
{
    public interface IPatientService
    {
        Task<ApiResponse<IEnumerable<PatientResponseDTO>>> GetAllAsync();
        Task<ApiResponse<PatientResponseDTO>> GetByIdAsync(Guid id);
        Task<ApiResponse<PatientResponseDTO>> CreatePatientAsync(CreatePatientDTO dto, Guid relativeId);
        Task<ApiResponse<PatientResponseDTO>> UpdatePatientAsync(UpdatePatientDTO dto, Guid relativeId);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
        Task<ApiResponse<IEnumerable<PatientReportDTO>>> GetReport(int fromAge, int toAge, CareServiceType servicetype, string name);
    }

}
