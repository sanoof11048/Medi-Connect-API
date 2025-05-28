using AutoMapper;
using Medi_Connect.Domain.DTOs.Auth;
using Medi_Connect.Domain.DTOs.UserDTO;
using Medi_Connect.Domain.DTOs.UserDTOs;
using Medi_Connect.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDTO , User>().ReverseMap();
            CreateMap<User, AuthResponseDTO>().ReverseMap();
            CreateMap<NurseProfile, NurseProfileCreateDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<CreateNurseDTO, User>()
                            .ForMember(dest => dest.NurseProfile, opt => opt.MapFrom(src => src.NurseProfile))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(_ => UserRole.Nurse));



        }
    }
}
