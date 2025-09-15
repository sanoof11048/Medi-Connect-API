using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.PatientDTO;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models.PatientDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Medi_Connect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        
        [HttpGet("{id}")]
        //[Authorize]
        public async Task<IActionResult> GetPatientById(Guid id)
        {
            var response = await _patientService.GetByIdAsync(id);
            return Ok(response);
        }


        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAllPatients()
        {
            var response = await _patientService.GetAllAsync();
            return Ok( response);
        }

        [HttpPost]
        [Authorize(Roles ="Relative, Admin")]
        public async Task<IActionResult> CreatePatient([FromForm] CreatePatientDTO dto)
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userId, out Guid chiefId))
            {
                var response = await _patientService.CreatePatientAsync(dto, chiefId);
                return StatusCode(response.StatusCode, response);
            }

            return Unauthorized("User not authenticated.");

        }

        [HttpPut]
        [Authorize(Roles ="Relative")]
        public async Task<IActionResult> UpdatePatient([FromForm] UpdatePatientDTO dto)
        {
            if (HttpContext.Items["UserId"] is Guid chiefId)
            {
                var response = await _patientService.UpdatePatientAsync(dto, chiefId);
                return StatusCode(response.StatusCode, response);
            }

            return Unauthorized("User not authenticated.");
        }

        [HttpDelete("{id}")]
        //[Authorize]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var response = await _patientService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("getReport")]
        public async Task<IActionResult> GetReport(int fromAge, int toAge, CareServiceType servicetype, string? name)
        {
            var res = await _patientService.GetReport(fromAge, toAge, servicetype, name);
            return StatusCode(res.StatusCode, res);
        }
    }
}
