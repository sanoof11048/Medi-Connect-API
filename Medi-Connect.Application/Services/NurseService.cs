using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Domain.DTOs.UserDTOs;
using Medi_Connect.Domain.Models;
using Medi_Connect.Domain.Models.ApiResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Services
{
    public class NurseService
    {
        //private readonly IGenericRepository<NurseProfile> _geneRepo;
        //private readonly IAdminRepository _adminRepo;
        //public NurseService(IGenericRepository<NurseProfile> geneRepo, IAdminRepository adminRepo)
        //{
        //    _geneRepo = geneRepo;
        //    _adminRepo = adminRepo;
        //}
        //public async Task<ApiResponse<string>> AddNurseProfile(CreateNurseDTO nurseProfile)
        //{
        //    if (nurseProfile == null)
        //    {
        //        return new ApiResponse<string>(400,"Invalid Data");
        //    }
        //    await _adminRepo.AddNurseAsync(nurseProfile);
        //    return new ApiResponse<string>(200, "Profile Updated Successfully");
        //}
    }
}
