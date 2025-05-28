using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.Auth;
using Medi_Connect.Domain.DTOs.UserDTO;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Medi_Connect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var response = await _authService.Register(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            var response = await _authService.Login(request);


            if (!response.Success)
                return Unauthorized(response);

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var response = await _authService.RefreshToken(request.RefreshToken);

            if (!response.Success)
                return Unauthorized(response);

            return Ok(response);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleAuthDTO request)
        {
            var response = await _authService.GoogleLogin(request.IdToken);

            if (!response.Success)
                return Unauthorized(response);

            return Ok(response);
        }


    }
}
