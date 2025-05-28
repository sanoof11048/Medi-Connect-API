using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.UserDTOs;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CloudinaryDotNet;
using Medi_Connect.Application.Interfaces;

namespace Medi_Connect.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IGenericRepository<User> _geneRepo;
        public AdminService(IAdminRepository adminRepository, IMapper mapper, ICloudinaryService cloudinaryService, IGenericRepository<User> genericRepository)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            _geneRepo = genericRepository;
            _cloudinaryService = cloudinaryService;
        }   

        public async Task<ApiResponse<List<User>>> GetAllNurses()
        {
            try
            {
            var nurses = await _adminRepository.GetAllNurses();
            return new ApiResponse<List<User>>(200, "Nurses Fetched Success", nurses, null);

            }
            catch (Exception ex)
            {
                return new ApiResponse<List<User>>(500, ex.Message);
            }
        }

        public async Task<ApiResponse<User>> AddNurse(CreateNurseDTO nurseDTO)
        {
            if (nurseDTO == null)
                return new ApiResponse<User>(400, "Bad Request", null , "Invalid data");
            
            var existingUser = await _adminRepository.GetUserByEmail(nurseDTO.Email);
            if (existingUser != null)
                return new ApiResponse<User>(400, "User Exists", null, "Use a different email");

            nurseDTO.Password = BCrypt.Net.BCrypt.HashPassword(nurseDTO.Password);

            if(nurseDTO.PhotoFile != null)
            {
                var imageurl = await _cloudinaryService.UploadImageAsync(nurseDTO.PhotoFile);
                nurseDTO.PhotoUrl = imageurl;
            }

            if (nurseDTO.NurseProfile?.CertificateFiles != null && nurseDTO.NurseProfile.CertificateFiles.Any())
            {
                var certUrls = await _cloudinaryService.UploadMultipleImagesAsync(nurseDTO.NurseProfile.CertificateFiles);
                nurseDTO.NurseProfile.CertificatesUrls = certUrls;
            }

            var user = _mapper.Map<User>(nurseDTO);
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            user.Role = UserRole.Nurse;


            if (user.NurseProfile != null)
            {
                user.NurseProfile.Id = Guid.NewGuid();
                user.NurseProfile.UserId = user.Id;
            }
            

            await _adminRepository.AddNurseAsync(user);
            return new ApiResponse<User>(200, "Nurse Added", user);
        }

        public async Task<ApiResponse<string>> DeleteNurse(Guid id)
        {
            var user =  await _geneRepo.GetByIdAsync(id);
            await _geneRepo.DeleteAsync(user);
            return new ApiResponse<string>(200, "deleted");
        }

        public async Task<ApiResponse<string>> UpdateNurse(CreateNurseDTO nurseDTO)
        {
            if (nurseDTO == null || string.IsNullOrEmpty(nurseDTO.Email))
                return new ApiResponse<string>(400, "Bad Request", null, "Invalid data");


            var existingUser = await _adminRepository.GetUserByEmail(nurseDTO.Email);
            if (existingUser == null)
                return new ApiResponse<string>(404, "User not found", null, "No user found with the given email");

            existingUser.FullName = nurseDTO.FullName ?? existingUser.FullName;
            existingUser.PhoneNumber = nurseDTO.PhoneNumber ?? existingUser.PhoneNumber;

            if (!string.IsNullOrEmpty(nurseDTO.Password))
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(nurseDTO.Password);
                existingUser.Password = hashedPassword;
            }

            if (nurseDTO.PhotoFile != null) 
                existingUser.Photo_url = await _cloudinaryService.UploadImageAsync(nurseDTO.PhotoFile);

            if (existingUser.NurseProfile != null && nurseDTO.NurseProfile != null)
            {
                existingUser.NurseProfile.Qualification = nurseDTO.NurseProfile.Qualification ?? existingUser.NurseProfile.Qualification;
                existingUser.NurseProfile.ExperienceYears = nurseDTO.NurseProfile.ExperienceYears;
                existingUser.NurseProfile.Bio = nurseDTO.NurseProfile.Bio ?? existingUser.NurseProfile.Bio;

                if (nurseDTO.NurseProfile.CertificateFiles != null && nurseDTO.NurseProfile.CertificateFiles.Count > 0)
                {
                    var certUrls = await _cloudinaryService.UploadMultipleImagesAsync(nurseDTO.NurseProfile.CertificateFiles);
                    existingUser.NurseProfile.CertificatesUrls = certUrls;
                }
            }

            await _geneRepo.UpdateAsync(existingUser);

            return new ApiResponse<string>(200, "Nurse profile updated successfully");
        }

    }
}
