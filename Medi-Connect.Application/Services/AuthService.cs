using AutoMapper;
using Google.Apis.Auth;
using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.Auth;
using Medi_Connect.Domain.DTOs.UserDTO;
using Medi_Connect.Domain.Models;
using Medi_Connect.Domain.Models.ApiResponses;
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
        private readonly IGenericRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(IGenericRepository<User> userRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ApiResponse<AuthResponseDTO>> Login(LoginDTO request)
        {
            var user = (await _userRepository.GetAllAsync())
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


            var accessToken = GenerateToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);

            var responseDto = _mapper.Map<AuthResponseDTO>(user);
            responseDto.AccessToken = accessToken;
            responseDto.RefreshToken = refreshToken;

            return new ApiResponse<AuthResponseDTO>(200, "Login successful", responseDto);
        }

        public async Task<ApiResponse<AuthResponseDTO>> Register(RegisterDTO userDto)
        {
            if (userDto == null)
                return new ApiResponse<AuthResponseDTO>(400, "Bad Request", null, "Invalid data");

            var existingUser = (await _userRepository.GetAllAsync())
                        .FirstOrDefault(u => u.Email == userDto.Email);

            if (existingUser != null)
                return new ApiResponse<AuthResponseDTO>(400, "User Exists", null, "Use a different email");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            userDto.Password = hashedPassword;


            var newUser = _mapper.Map<User>(userDto);
            await _userRepository.AddAsync(newUser);

            var accessToken = GenerateToken(newUser);
            var refreshToken = GenerateRefreshToken();

            newUser.RefreshToken = refreshToken;
            newUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(newUser);

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

                var existingUser = (await _userRepository.GetAllAsync())
                    .FirstOrDefault(u => u.Email == payload.Email);

                User user;

                if (existingUser == null)
                {
                    user = new User
                    {
                        Id = Guid.NewGuid(),
                        FullName = payload.Name,
                        Email = payload.Email,
                        Photo_url = payload.Picture,
                        Role = UserRole.Relative, 
                        IsActive = true
                    };

                    await _userRepository.AddAsync(user);
                }
                else
                {
                    user = existingUser;
                }

                var accessToken = GenerateToken(user);
                var refreshToken = GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _userRepository.UpdateAsync(user);

                var responseDto = _mapper.Map<AuthResponseDTO>(user);
                responseDto.AccessToken = accessToken;
                responseDto.RefreshToken = refreshToken;

                return new ApiResponse<AuthResponseDTO>(200, "Google login successful", responseDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthResponseDTO>(401, "Google login failed", null, ex.Message);
            }
        }


        public async Task<ApiResponse<AuthResponseDTO>> RefreshToken(string refreshToken)
        {
            var user = (await _userRepository.GetAllAsync())
                        .FirstOrDefault(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new ApiResponse<AuthResponseDTO>(401, "Invalid or expired refresh token");
            }

            var newAccessToken = GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);

            var responseDto = _mapper.Map<AuthResponseDTO>(user);
            responseDto.AccessToken = newAccessToken;
            responseDto.RefreshToken = newRefreshToken;

            return new ApiResponse<AuthResponseDTO>(200, "Token refreshed successfully", responseDto);
        }



        private string GenerateToken(User user)
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

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashedPassword);
        }
    }
}
