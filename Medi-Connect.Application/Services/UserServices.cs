using AutoMapper;
using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.UserDTO;
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
    public class UserServices: IUserService
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
      
        public async Task<IEnumerable<User>> GetAllAsync()
        {

            var users = await _geneRepo.GetAllAsync();
            return users;
        }

        public async Task<ApiResponse<string>> ToggleBlockUser(Guid id)
        {
            var user = await _geneRepo.GetByIdAsync(id);

            if (user == null)
                return new ApiResponse<string>(404, "User Not Found", null, "No user found with the provided ID.");
            

            user.IsActive = !user.IsActive;

            await _geneRepo.UpdateAsync(user);

            var status = user.IsActive ? "unblocked" : "blocked";
            return new ApiResponse<string>(200, "Success", $"User has been successfully {status}.");
        }

        public async Task<ApiResponse<User>> GetById(Guid id)
        {
            var user = await _geneRepo.GetByIdAsync(id);

            if (user == null)
                return new ApiResponse<User>(404, "User Not Found", null, "No user found with the provided ID.");
            
            return new ApiResponse<User>(200, "User Found", user, null);
        }

        public async Task<ApiResponse<string>> DeleteUser(Guid id)
        {
            var user = await _geneRepo.GetByIdAsync(id);

            if (user == null)
            {
                return new ApiResponse<string>(404, "User Not Found", null, "No user found with the provided ID.");
            }
            await _geneRepo.DeleteAsync(user);
            return new ApiResponse<string>(200,  "User Deleted", null);

        }
    }
}
