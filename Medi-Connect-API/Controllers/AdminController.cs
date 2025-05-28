using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.UserDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medi_Connect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("GetAllNurses")]
        public async Task<IActionResult> GetAllNurses()=>
            Ok(await _adminService.GetAllNurses());


        [HttpPost("AddNurse")]
        public async Task<IActionResult> AddNurse(CreateNurseDTO nurseDTO )=>
            Ok(await _adminService.AddNurse(nurseDTO));
        

        [HttpDelete("{id}/deleteNurse")]
        public async Task<IActionResult> DeleteNurse(Guid id)=>
            Ok(await _adminService.DeleteNurse(id));



    }
}
