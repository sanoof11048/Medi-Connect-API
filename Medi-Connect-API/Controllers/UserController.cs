using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Application.Services;
using Medi_Connect.Domain.DTOs.UserDTO;
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
        public UserController( IUserService userService)
        {
            _userService = userService;
        }

        //[Authorize]
        [HttpGet("")]
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
    }
}
