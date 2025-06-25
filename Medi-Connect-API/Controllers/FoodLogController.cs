using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.PatientDTO.FoodLogDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medi_Connect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodLogController : ControllerBase
    {
        private readonly IFoodLogService _foodLogService;
        public FoodLogController(IFoodLogService foodLogService)
        {
            _foodLogService = foodLogService;
        }

        [HttpPost]
        [Authorize(Roles = "HomeNurse")]
        public async Task<IActionResult> AddFoodLog([FromBody] FoodLogCreateDTO createDTO)=>
            Ok(await _foodLogService.AddFoodLogAsync(createDTO));


        [HttpGet("{mealId}")]
        public async Task<IActionResult> GetById(Guid mealId)=>
            Ok(await _foodLogService.GetByIdAsync(mealId));


        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(Guid patientId)=>
            Ok(await _foodLogService.GetByPatientIdAsync(patientId));


        [HttpPut]
        [Authorize(Roles = "HomeNurse")]
        public async Task<IActionResult> Update([FromBody] FoodLogUpdateDTO dto)=>
            Ok(await _foodLogService.UpdateAsync(dto));


        [HttpDelete("{mealId}")]
        [Authorize(Roles = "HomeNurse")]
        public async Task<IActionResult> Delete(Guid mealId)=>
            Ok(await _foodLogService.DeleteAsync(mealId));

    }
}
