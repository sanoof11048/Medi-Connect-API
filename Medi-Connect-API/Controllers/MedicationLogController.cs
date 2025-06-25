using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.PatientDTO.MedicationLogDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medi_Connect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicationLogController : ControllerBase
    {
        private readonly IMedicationLogService _service;

        public MedicationLogController(IMedicationLogService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "HomeNurse")]
        public async Task<IActionResult> Add([FromBody] MedicationLogCreateDTO dto) =>
            Ok(await _service.AddMedicationLogAsync(dto));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) =>
            Ok(await _service.GetByIdAsync(id));

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(Guid patientId) =>
            Ok(await _service.GetByPatientIdAsync(patientId));

        [HttpPut]
        [Authorize(Roles = "HomeNurse")]
        public async Task<IActionResult> Update([FromBody] MedicationLogUpdateDTO dto) =>
            Ok(await _service.UpdateAsync(dto));

        [HttpDelete("{id}")]
        [Authorize(Roles = "HomeNurse")]
        public async Task<IActionResult> Delete(Guid id) =>
            Ok(await _service.DeleteAsync(id));
    }

}
