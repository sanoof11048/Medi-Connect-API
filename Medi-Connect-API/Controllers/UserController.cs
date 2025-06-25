using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Application.Services;
using Medi_Connect.Domain.DTOs.PatientDTO;
using Medi_Connect.Domain.DTOs.ReportDTOs;
using Medi_Connect.Domain.Models;
using Medi_Connect.Domain.Models.ApiResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medi_Connect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IReportService _reportService;
        public UserController( IUserService userService, IReportService report)
        {
            _userService = userService;
            _reportService = report;
        }

        //[Authorize]
        [HttpGet("all-user")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        //[Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }


        //[Authorize(Roles = "Admin")]
        [HttpPatch("{id}/toggle-block")]
        public async Task<IActionResult> ToggleBlockUser(Guid id)
        {
            var response = await _userService.ToggleBlockUser(id);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var response = await _userService.DeleteUser(id); 
            return Ok(response);

        }

        [HttpPost("download-report")]
        [Authorize]
        public async Task<IActionResult> DownloadReport([FromBody] PatientReportRequestDTO request)
        {
            try
            {
                var report = await _userService.GeneratePatientReportAsync(request);
                //var pdfBytes = _reportService.GenerateReportPdf(report);

                var fileName = $"Patient_Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.pdf";

                //return File( "application/pdf", fileName);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error generating report", Details = ex.Message });
            }
        }
    }
}
