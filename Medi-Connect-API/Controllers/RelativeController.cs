using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Application.Services;
using Medi_Connect.Domain.DTOs.PatientDTO;
using Medi_Connect.Domain.DTOs.RelativeDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Medi_Connect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelativeController : ControllerBase
    {
        private readonly IRelativeService _relativeService;

        public RelativeController(IRelativeService relativeService)
        {
            _relativeService = relativeService;
        }
        [Authorize]
        [HttpGet("get-patient-of-relative")]
        public async Task<IActionResult> GetPatientByRelative()
        {
            try
            {
                var userIdStr = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userIdStr))
                    return Unauthorized("User ID claim is missing.");

                if (!Guid.TryParse(userIdStr, out Guid userId))
                    return BadRequest("Invalid user ID format");

                var result = await _relativeService.GetPatientOfRelative(userId);

                if (result == null || result.Data == null || !result.Data.Any())
                    return NotFound("No patient found for this relative.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }




        [Authorize(Roles ="Admin")]
        [HttpGet("GetAllRelatives")]
        public async Task<IActionResult> GetAllReltives()
        {
            return Ok(await _relativeService.GetAllRelatives());
        }
    }
}
