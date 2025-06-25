using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.NurseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medi_Connect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly INurseService _nurseService;
        public AdminController(IAdminService adminService, INurseService nurseService)
        {
            _adminService = adminService;
            _nurseService = nurseService;
        }

        [HttpGet("GetAllNurses")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()=>
            Ok(await _nurseService.GetAllNurseProfilesAsync());


        [HttpGet("getnursebyid/{id:guid}")]
        [Authorize(Roles = "Admin,HomeNurse,Relative")]
        public async Task<IActionResult> GetById(Guid id)=>
            Ok(await _nurseService.GetNurseProfileAsync(id));


        [HttpPost("AddNurse")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNurse([FromBody] NurseProfileCreateDTO nurseDTO)
        {
            Console.WriteLine($"Received Email: {nurseDTO.Email}");
            return Ok(await _nurseService.CreateNurseProfileAsync(nurseDTO));
        }



        [HttpDelete("{id}/deleteNurse")]
        [Authorize(Roles = "Admin,HomeNurse")]
        public async Task<IActionResult> DeleteNurse(Guid id)=>
            Ok(await _nurseService.DeleteNurseProfileAsync(id));

        [HttpPut("update-nurse/{id}")]
        [Authorize(Roles = "Admin, HomeNurse")]
        public async Task<IActionResult> UpdateNurse(Guid id, [FromBody] UpdateNurseProfileDTO nurseDTO)=>
            Ok(await _nurseService.UpdateNurseProfileAsync(id, nurseDTO));


        [HttpGet("GetAllPatients")]
        public async Task<IActionResult> GetAllPatients()=>
           Ok(await _adminService.GetAllPatients());
        
    }
}
