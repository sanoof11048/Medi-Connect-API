using Azure;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.Auth;
using Medi_Connect.Domain.DTOs.Auth.OTP;
using Medi_Connect.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;

namespace Medi_Connect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO dto)
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
            
            //if (!response.Success)
            //    return Unauthorized(response);

            return Ok(response);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleAuthDTO request)
        {
            var response = await _authService.GoogleLogin(request.IdToken);
            return Ok(response); 
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOTP(string email)
        {
            var response = await _authService.SendOTPAsync(email);
            if (response.StatusCode != 200)
            {
                return BadRequest(response);
            }
            await _emailService.SendEmailAsync(email, "OTP for Reset Password", $"Your OTP is: {response.Data}");
            return Ok(response);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDTO request)
        {
            var response = await _authService.VerifyOtp(request);
            if (response.StatusCode != 200)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ChangePasswordDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("Email is required.");

            if (!InMemoryOtpStore.OtpDict.TryGetValue(dto.Email, out var otpRecord))
                return BadRequest("OTP verification required.");
            var response = await _authService.ResetPassword(dto);
            return Ok(response);
        }

        //[Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userIdClaim = Guid.Parse(HttpContext.Items["UserId"].ToString());
            if (userIdClaim == null) return Unauthorized();

            var result = await _authService.LogoutAsync(userIdClaim);
            return Ok(result);
        }


    }
}
