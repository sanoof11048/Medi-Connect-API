using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Application.Services;
using Medi_Connect.Domain.DTOs.NurseDTO;
using Medi_Connect.Domain.Models.ApiResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medi_Connect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NurseController : ControllerBase
    {
        private readonly INurseService _nurseService;


        public NurseController(INurseService nurseService)
        {
            _nurseService = nurseService;
        }

        //[HttpPost("profile")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> AddNurseProfile([FromBody] NurseProfileCreateDTO nurseProfile)
        //{
        //    var response = await _nurseService.CreateNurseProfileAsync(nurseProfile);
        //    return Ok(response);
        //}

        [HttpGet("profile/{id}")]
        [Authorize(Roles = "Admin,HomeNurse")]
        public async Task<IActionResult> GetNurseProfile(Guid id)
        {
            var response = await _nurseService.GetNurseProfileAsync(id);
            return Ok(response);
        }

        [HttpGet("profiles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllNurseProfiles()
        {
            var response = await _nurseService.GetAllNurseProfilesAsync();
            return Ok(response);
        }

        [HttpPut("profile/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateNurseProfile(Guid id, [FromBody] UpdateNurseProfileDTO dto)
        {
            var response = await _nurseService.UpdateNurseProfileAsync(id, dto);
            return Ok(response);
        }

        [HttpDelete("profile/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteNurseProfile(Guid id)
        {
            var response = await _nurseService.DeleteNurseProfileAsync(id);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("getPatientsOfHomeNurse")]
        public async Task<IActionResult> GetPatientByRelative()
        {
            if (!HttpContext.Items.TryGetValue("UserId", out var userObj) || userObj == null || userObj is not Guid userId)
            {
                return Unauthorized("UserId not found in request context.");
            }

            return Ok(await _nurseService.GetPatientOfHomeNurse(userId));
        }
    }
    }