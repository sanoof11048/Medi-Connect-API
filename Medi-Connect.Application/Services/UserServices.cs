using AutoMapper;
using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.Auth;
using Medi_Connect.Domain.DTOs.ReportDTOs;
using Medi_Connect.Domain.DTOs.UserDTOs;
using Medi_Connect.Domain.Models;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models.Users;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Services
{
    public class UserServices : IUserService
    {
        private readonly IGenericRepository<User> _geneRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UserServices(IGenericRepository<User> genericRepository, IUserRepository userRepository, IMapper mapper)
        {
            _geneRepo = genericRepository;
            _userRepo = userRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<UserDTO>>> GetAllAsync()
        {

            var users = await _geneRepo.GetAllAsync();
            var res = _mapper.Map<IEnumerable<UserDTO>>(users);
            return new ApiResponse<IEnumerable<UserDTO>>(200, "Fetched",res , null);
        }

        public async Task<ApiResponse<string>> ToggleBlockUser(Guid id)
        {
            var user = await _geneRepo.GetByIdAsync(id);

            if (user == null || user.IsDeleted)
                return new ApiResponse<string>(404, "User Not Found", null, "No user found with the provided ID.");


            user.IsActive = !user.IsActive;

            await _geneRepo.UpdateAsync(user);

            var status = user.IsActive ? "unblocked" : "blocked";
            return new ApiResponse<string>(200, "Success", $"User has been successfully {status}.");
        }

        public async Task<ApiResponse<UserDTO>> GetById(Guid id)
        {
            var user = await _geneRepo.GetByIdAsync(id);

            if (user == null || user.IsDeleted)
                return new ApiResponse<UserDTO>(404, "User Not Found", null, "No user found with the provided ID.");
            var res = _mapper.Map<UserDTO>(user);
            return new ApiResponse<UserDTO>(200, "User Found", res, null);
        }

        public async Task<ApiResponse<string>> DeleteUser(Guid id)
        {
            var user = await _geneRepo.GetByIdAsync(id);

            if (user == null)
            {
                return new ApiResponse<string>(404, "User Not Found", null, "No user found with the provided ID.");
            }
            await _geneRepo.DeleteAsync(user);
            return new ApiResponse<string>(200, "User Deleted", null);

        }


        public async Task<ApiResponse<UserDTO>> UpdateUserAsync(UpdateUserDTO dto)
        {
            try
            {
                var user = await _geneRepo.GetByIdAsync(dto.Id);
                if (user == null)
                    return new ApiResponse<UserDTO>(404, "User Not Found");

                _mapper.Map(user,dto);

                await _geneRepo.UpdateAsync(user);

                return new ApiResponse<UserDTO>(200,"Profile Updated", null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserDTO>(500, "Something Went Wrong",null, ex.Message);
            }
        }

        public async Task<PatientReportDTO> GeneratePatientReportAsync(PatientReportRequestDTO request)
        {

            var report = await _userRepo.GeneratePatientReportAsync(request);
            return report;

        }
    }
}
