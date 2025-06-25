using Medi_Connect.Domain.DTOs.Auth;
using Medi_Connect.Domain.DTOs.Auth.OTP;
using Medi_Connect.Domain.Models;
using Medi_Connect.Domain.Models.ApiResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.ISerives
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponseDTO>> Login(LoginDTO request);
        Task<ApiResponse<AuthResponseDTO>> Register(RegisterDTO userDto);
        Task<ApiResponse<AuthResponseDTO>> RefreshToken(string refreshToken);
        Task<ApiResponse<AuthResponseDTO>> GoogleLogin(string idToken);
        Task<ApiResponse<string>> SendOTPAsync(string emailDTO);
        Task<ApiResponse<string>> VerifyOtp(VerifyOtpDTO verifyOtpDTO);
        Task<ApiResponse<string>> ResetPassword(ChangePasswordDTO passwordDTO);
        Task<ApiResponse<string>> LogoutAsync(Guid userId);

    }
}
