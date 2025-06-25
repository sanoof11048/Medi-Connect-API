using Medi_Connect.Domain.DTOs.NurseDTO;
using Medi_Connect.Domain.DTOs.PaymentDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.ISerives
{
    public interface INurseAssignmentService
    {
        Task<ApiResponse<NurseRequestResponseDTO>> SendRequestAsync(NurseRequestDTO dto, Guid userId);
        Task<ApiResponse<string>> AssignNurseAsync(AssignNurseDTO dto);
        Task<ApiResponse<string>> MarkPaymentAsync(MarkPaymentDTO dto);
        Task<ApiResponse<List<NurseAssignmentResponseDTO>>> GetAllAssignmentsAsync();
        Task<ApiResponse<string>> PayToAdminPartial(PartialPaymentDTO dto, Guid relativeId);
        Task<ApiResponse<IEnumerable<NurseRequestResponseDTO>>> GetAllRequests();

    }
}
