using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.PaymentDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medi_Connect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize(Roles = "Relative")]
        [HttpGet("my-payments")]
        public async Task<IActionResult> GetMyPayments()
        {
            var userId = (Guid)HttpContext.Items["UserId"];
            var response = await _paymentService.GetMyPayments(userId);
            return Ok(response);
        }
        [Authorize(Roles ="Relative")]
        [HttpPost("create-razorpay-order")]
        public async Task<IActionResult> CreateRazorpayOrder([FromBody] PartialPaymentDTO dto)
        {
            var userId = (Guid)HttpContext.Items["UserId"];
            if (dto.Amount <= 0)
                return BadRequest(new ApiResponse<string>(400, "Amount must be greater than 0"));

            var result = await _paymentService.InitializeRazorpayPayment(dto, userId);
            if (!result.Success)
                return BadRequest(new ApiResponse<string>(400, "Payment init failed"));

            return Ok(new ApiResponse<object>(200, "Order created", result.Data));
        }
        [Authorize(Roles = "Relative")]
        [HttpPost("verify-razorpay-payment")]
        public async Task<IActionResult> VerifyRazorpayPayment([FromBody] VerifyPaymentDTO dto)
        {
            var userId = (Guid)HttpContext.Items["UserId"];
            var result = await _paymentService.VerifyAndRecordPayment(dto, userId);

            if (!result.Success)
                return BadRequest(new ApiResponse<string>(400, "Verification Failed"));

            return Ok(result);
        }



    }
}
