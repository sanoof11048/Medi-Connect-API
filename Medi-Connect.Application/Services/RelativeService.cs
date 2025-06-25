using AutoMapper;
using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.PatientDTO;
using Medi_Connect.Domain.DTOs.RelativeDTO;
using Medi_Connect.Domain.DTOs.UserDTOs;
using Medi_Connect.Domain.Models.ApiResponses;
using Medi_Connect.Domain.Models.Other;
using Medi_Connect.Domain.Models.PatientDetails;
using Medi_Connect.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Services
{
    public class RelativeService : IRelativeService
    {
        private readonly IGenericRepository<Patient> _genePatientRepo;
        private readonly IPatientRepository _patientRepository;
        private readonly INurseRequestRepository _nurseAssignmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public RelativeService(IGenericRepository<Patient> genericRepository, IPatientRepository patientRepository, IMapper mapper, INurseRequestRepository nurseAssignmentRepository, IUserRepository userRepository)
        {
            _genePatientRepo = genericRepository;
            _patientRepository = patientRepository;
            _mapper = mapper;
            _nurseAssignmentRepository = nurseAssignmentRepository;
            _userRepository = userRepository;
        }
        public async Task<ApiResponse<IEnumerable<PatientResponseDTO>>> GetPatientOfRelative(Guid userId)
        {
            try
            {
                var patients = await _patientRepository.GetPatientsByRelativeId(userId);
                if (patients == null || !patients.Any())
                {
                    return new ApiResponse<IEnumerable<PatientResponseDTO>>(404, "No More Patients Found");
                }

                var patientDTOs = _mapper.Map<IEnumerable<PatientResponseDTO>>(patients);

                return new ApiResponse<IEnumerable<PatientResponseDTO>>(200, "Patients Fetched", patientDTOs);
            }
            catch (Exception ex) 
            {
                return new ApiResponse<IEnumerable<PatientResponseDTO>>(500, ex.Message, null, ex.ToString());
            }
        }

        public async Task<ApiResponse<IEnumerable<UserDTO>>> GetAllRelatives()
        {
            try
            {
                var users = await _userRepository.GetAllRelativesAsync();
                var usersList =  _mapper.Map<IEnumerable<UserDTO>>(users);
                return new ApiResponse<IEnumerable<UserDTO>>(200, "Relatives Fetched", usersList, null);

            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<UserDTO>>(500, ex.Message, null, ex.ToString());
            }
        }
    }
}
