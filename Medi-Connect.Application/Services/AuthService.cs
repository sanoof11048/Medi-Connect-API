using AutoMapper;
using Google.Apis.Auth;
using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.Auth;
using Medi_Connect.Domain.DTOs.Auth.OTP;
using Medi_Connect.Domain.DTOs.UserDTOs;
using Medi_Connect.Domain.Models;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _geneRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ICloudinaryService _cloudinaryService;

        public AuthService(IGenericRepository<User> geneRepository, IConfiguration configuration, IMapper mapper, IUserRepository userRepository, ICloudinaryService cloudinaryService)
        {
            _geneRepository = geneRepository;
            _configuration = configuration;
            _mapper = mapper;
            _userRepository = userRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ApiResponse<AuthResponseDTO>> Login(LoginDTO request)
        {
            var user = (await _geneRepository.GetAllAsync())
                        .FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                return new ApiResponse<AuthResponseDTO>(401, "Invalid Email");
            }
            if (!VerifyPassword(request.Password, user.Password))
            {
                return new ApiResponse<AuthResponseDTO>(401, "Invalid Password");
            }
            if (!user.IsActive)
            {
                return new ApiResponse<AuthResponseDTO>(401, "User Blocked", error: "Your account has been blocked. Please contact support.");
            }


            var accessToken = await GenerateToken(user);
            string refreshToken;
            if (user.RefreshToken != null && user.RefreshTokenExpiryTime > DateTime.UtcNow)
            {
                refreshToken = user.RefreshToken; 
            }
            else
            {
                refreshToken = await GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _geneRepository.UpdateAsync(user);
            }

            var responseDto = _mapper.Map<AuthResponseDTO>(user);
            responseDto.AccessToken = accessToken;
            responseDto.RefreshToken = refreshToken;

            return new ApiResponse<AuthResponseDTO>(200, "Login successful", responseDto);
        }

        public async Task<ApiResponse<AuthResponseDTO>> Register(RegisterDTO userDto)
        {
            if (userDto == null)
                return new ApiResponse<AuthResponseDTO>(400, "Bad Request", null, "Invalid data");

            var existingUser = await _userRepository.GetUserByEmail(userDto.Email);

            if (existingUser != null)
                return new ApiResponse<AuthResponseDTO>(400, "User Already Exists", null, "Use a different email");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            userDto.Password = hashedPassword;


            var newUser = _mapper.Map<User>(userDto);
            newUser.Id = Guid.NewGuid();

            if (userDto.PhotoFile != null)
            {
                var photoUrl = await _cloudinaryService.UploadImageAsync(userDto.PhotoFile);
                newUser.PhotoUrl = photoUrl;
            }

            var accessToken = await GenerateToken(newUser);
            var refreshToken = await GenerateRefreshToken();

            newUser.RefreshToken = refreshToken;
            newUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _geneRepository.AddAsync(newUser);

            var responseDto = _mapper.Map<AuthResponseDTO>(newUser);
            responseDto.AccessToken = accessToken;
            responseDto.RefreshToken = refreshToken;


            return new ApiResponse<AuthResponseDTO>(200, "Success", responseDto, null);
        }
        public async Task<ApiResponse<AuthResponseDTO>> GoogleLogin(string idToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { _configuration["GoogleAuthSettings:ClientId"] }
                });

                var existingUser = await _userRepository.GetUserByEmail(payload.Email);

                User user;

                if (existingUser == null)
                {
                    user = new User
                    {
                        Id = Guid.NewGuid(),
                        FullName = payload.Name,
                        Email = payload.Email,
                        PhotoUrl = payload.Picture,
                        Role = UserRole.Relative,
                        IsActive = true
                    };

                    await _geneRepository.AddAsync(user);
                }
                else
                {
                    user = existingUser;
                }

                if (user.PhotoUrl != payload.Picture)
                {
                    user.PhotoUrl = payload.Picture;
                }


                var accessToken = await GenerateToken(user);
                string refreshToken;

                if (user.RefreshToken != null && user.RefreshTokenExpiryTime > DateTime.UtcNow)
                {
                    refreshToken = user.RefreshToken; // Reuse existing token
                }
                else
                {
                    refreshToken = await GenerateRefreshToken();
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                    await _geneRepository.UpdateAsync(user);
                }


                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _geneRepository.UpdateAsync(user);

                var responseDto = _mapper.Map<AuthResponseDTO>(user);
                responseDto.AccessToken = accessToken;
                responseDto.RefreshToken = refreshToken;

                return new ApiResponse<AuthResponseDTO>(200, "Google login successful", responseDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthResponseDTO>(500, "Google login failed", null, ex.Message);
            }
        }


        public async Task<ApiResponse<AuthResponseDTO>> RefreshToken(string refreshToken)
        {
            var user = (await _geneRepository.GetAllAsync())
                        .FirstOrDefault(u => u.RefreshToken == refreshToken);

            if(user == null)
            {
                return new ApiResponse<AuthResponseDTO>(401, "No user Found with this Token");
            }
            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new ApiResponse<AuthResponseDTO>(401, "Invalid or expired refresh token");
            }

            var newAccessToken = await GenerateToken(user);
            var newRefreshToken = await GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _geneRepository.UpdateAsync(user);

            var responseDto = _mapper.Map<AuthResponseDTO>(user);
            responseDto.AccessToken = newAccessToken;
            responseDto.RefreshToken = newRefreshToken;

            return new ApiResponse<AuthResponseDTO>(200, "Token refreshed successfully", responseDto);
        }



        private async Task<string> GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private Task<string> GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Task.FromResult(Convert.ToBase64String(randomBytes));
        }

        private bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashedPassword);
        }


        public async Task<ApiResponse<string>> SendOTPAsync(string emailDTO)
        {
            var user = await _userRepository.GetUserByEmail(emailDTO);
            if (user == null)
            {
                return new ApiResponse<string>(400, "Invalid Email", "Incorrect Email", "User Not Found");
            }
            var otp = new Random().Next(100000, 999999).ToString();
            await _userRepository.AddtoOtpStore(emailDTO, otp);

            return new ApiResponse<string>(200, "OTP generated successfully", otp);
        }

        public async Task<ApiResponse<string>> VerifyOtp(VerifyOtpDTO verifyOtpDTO)
        {

            string isVerified = await _userRepository.VerifyOtp(verifyOtpDTO.Email, verifyOtpDTO.Otp);
            if (isVerified != "Otp Verified")
            {
                return new ApiResponse<string>(400, isVerified);
            }
            return new ApiResponse<string>(200, isVerified);
        }

        public async Task<ApiResponse<string>> ResetPassword(ChangePasswordDTO passwordDTO)
        {
            var user = await _userRepository.GetUserByEmail(passwordDTO.Email);
            if (user == null)
            {
                return new ApiResponse<string>(400, "User Not Found");
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(passwordDTO.NewPassword);
            await _geneRepository.UpdateAsync(user);
            return new ApiResponse<string>(200, "Password Updated successfully");


        }

        public async Task<ApiResponse<string>> LogoutAsync(Guid userId)
        {
            try
            {

            var user = await _geneRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse<string>(404, "User not found");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _geneRepository.UpdateAsync(user);

            return new ApiResponse<string>(200, "Logout successful");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, ex.Message, "Something Went Wrong");
            }
        }

    }

}
