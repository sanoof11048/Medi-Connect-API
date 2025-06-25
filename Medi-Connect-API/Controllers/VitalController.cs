using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.PatientDTO.VitalsDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medi_Connect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VitalController : ControllerBase
    {
        private readonly IVitalService _vitalService;
        public VitalController(IVitalService vitalService)
        {
            _vitalService = vitalService;
        }

        [HttpPost("vital")]
        [Authorize(Roles = "HomeNurse")]
        public async Task<IActionResult> CreateVital([FromBody] CreateVitalsDTO dto)
        {

            var response = await _vitalService.CreateVitalAsync(dto);
            return Ok(response);
        }

        [HttpGet("vitals/patient/{patientId}")]
        [Authorize]
        public async Task<IActionResult> GetVitalsByPatient(Guid patientId)
        {
            var response = await _vitalService.GetVitalsByPatientIdAsync(patientId);
            return Ok(response);
        }

        [HttpGet("vital/{id}")]
        [Authorize]
        public async Task<IActionResult> GetVitalById(Guid id)
        {
            var response = await _vitalService.GetVitalByIdAsync(id);
            return Ok(response);
        }

        [HttpPut("vital")]
        [Authorize(Roles = "HomeNurse")]
        public async Task<IActionResult> UpdateVital([FromBody] VitalUpdateDTO dto)
        {
            var response = await _vitalService.UpdateVitalAsync(dto);
            return Ok(response);
        }
        [HttpDelete("vital/{id}")]
        [Authorize(Roles = "HomeNurse")]
        public async Task<IActionResult> DeleteVital(Guid id)
        {
            var result = await _vitalService.DeleteVitalAsync(id);
            return Ok(result);
        }

    }
}
