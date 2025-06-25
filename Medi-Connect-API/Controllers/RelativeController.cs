using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Application.Services;
using Medi_Connect.Domain.DTOs.PatientDTO;
using Medi_Connect.Domain.DTOs.RelativeDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var userId = (Guid)HttpContext.Items["UserId"];
            return Ok(await _relativeService.GetPatientOfRelative(userId));
        }
        [Authorize(Roles ="Admin")]
        [HttpGet("GetAllRelatives")]
        public async Task<IActionResult> GetAllReltives()
        {
            return Ok(await _relativeService.GetAllRelatives());
        }
    }
}
