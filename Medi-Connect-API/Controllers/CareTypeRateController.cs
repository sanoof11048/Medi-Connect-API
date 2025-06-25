using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.PaymentDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medi_Connect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CareTypeRateController : ControllerBase
    {
        private readonly ICareTypeRateService _service;

        public CareTypeRateController(ICareTypeRateService service)
        {
            _service = service;
        }

        [HttpGet("GetAllPlans")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _service.GetByIdAsync(id);
            return res is null ? NotFound() : Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CareTypeRateCreateDTO dto) =>
            Ok(await _service.CreateAsync(dto));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CareTypeRateUpdateDTO dto) =>
            Ok(await _service.UpdateAsync(dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) =>
            Ok(await _service.DeleteAsync(id));
    }

}
