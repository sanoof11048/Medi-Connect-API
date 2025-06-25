using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.NurseDTO;
using Medi_Connect.Domain.DTOs.PaymentDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medi_Connect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NurseRequestController : ControllerBase
    {
        private readonly INurseAssignmentService _nurseAssignmentService;

        public NurseRequestController(INurseAssignmentService nurseAssignmentService)
        {
            _nurseAssignmentService = nurseAssignmentService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Relative")]
        public async Task<ActionResult<ApiResponse<NurseRequestResponseDTO>>> SendRequest(NurseRequestDTO dto)
        {
            var userId = (Guid)HttpContext.Items["UserId"];
            var result = await _nurseAssignmentService.SendRequestAsync(dto, userId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("assign")]
        public async Task<IActionResult> Assign([FromBody] AssignNurseDTO dto)
        {
            var response = await _nurseAssignmentService.AssignNurseAsync(dto);
            return Ok(response);
        }

        //[Authorize(Roles = "Admin")]
        //[HttpPost("payment")]
        //public async Task<IActionResult> MarkPayment([FromBody] MarkPaymentDTO dto)
        //{
        //    var response = await _nurseAssignmentService.MarkPaymentAsync(dto);
        //    return StatusCode(response.StatusCode, response);
        //}

        [Authorize(Roles = "Admin")]
        [HttpGet("assignments")]
        public async Task<IActionResult> GetAssignments()
        {
            var response = await _nurseAssignmentService.GetAllAssignmentsAsync();
            return Ok(response);
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllRequestsForAdmin()
        {
            var result = await _nurseAssignmentService.GetAllRequests();
            return Ok(result);
        }

    }
}
