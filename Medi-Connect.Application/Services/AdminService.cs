using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Domain.Models.ApiResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medi_Connect.Application.Interfaces.ISerives;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CloudinaryDotNet;
using Medi_Connect.Domain.DTOs.NurseDTO;
using Medi_Connect.Domain.DTOs.PatientDTO;
using Medi_Connect.Domain.Models.Users;
using Medi_Connect.Domain.Models.PatientDetails;

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

        public async Task<ApiResponse<ICollection<Patient>>> GetAllPatients()
        {
            try
            {
                var patients = await _adminRepository.GetAllPatients();
                //var patients = _mapper.Map<ICollection<Patient>>(res);
                if (patients != null)
                {
                    return new ApiResponse<ICollection<Patient>>(200, "Patients Fetched", patients);
                }
                return new ApiResponse<ICollection<Patient>>(404, "Not Found", null);

            }
            catch (Exception ex)
            {
                return new ApiResponse<ICollection<Patient>>(500,ex.Message, null, "Server Error");
            }
        }

    }
}
