using Medi_Connect.Domain.DTOs.PaymentDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.ISerives
{
    public interface IPaymentService
    {
        Task<ApiResponse<List<PendingPaymentDTO>>> GetMyPayments(Guid relativeId);
        Task<ApiResponse<string>> RecordPaymentAfterVerification(VerifyPaymentDTO dto, Guid relativeId);
        Task<ApiResponse<object>> InitializeRazorpayPayment(PartialPaymentDTO dto, Guid userId);
        Task<ApiResponse<string>> VerifyAndRecordPayment(VerifyPaymentDTO dto, Guid userId);


        //Task<ApiResponse<string>> PayNurseSalary(PaySalaryDTO dto, Guid adminId);
        //Task<ApiResponse<List<PendingPaymentDTO>>> GetPendingPaymentsForAdmin();
    }
}
