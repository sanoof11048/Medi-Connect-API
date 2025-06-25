using AutoMapper;
using Medi_Connect.Domain.Common;
using Medi_Connect.Domain.DTOs.Auth;
using Medi_Connect.Domain.DTOs.NurseDTO;
using Medi_Connect.Domain.DTOs.PatientDTO;
using Medi_Connect.Domain.DTOs.PatientDTO.FoodLogDTOs;
using Medi_Connect.Domain.DTOs.PatientDTO.MedicationLogDTOs;
using Medi_Connect.Domain.DTOs.PatientDTO.VitalsDTOs;
using Medi_Connect.Domain.DTOs.PaymentDTOs;
using Medi_Connect.Domain.DTOs.UserDTOs;
using Medi_Connect.Domain.Models.Admin;
using Medi_Connect.Domain.Models.Other;
using Medi_Connect.Domain.Models.PatientDetails;
using Medi_Connect.Domain.Models.Users;

namespace Medi_Connect.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDTO, User>().ReverseMap();
            CreateMap<User, AuthResponseDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UpdateUserDTO, User>().ReverseMap();

            CreateMap<NurseProfile, NurseProfileCreateDTO>().ReverseMap();
            CreateMap<NurseProfileCreateDTO, User>().ReverseMap();
            CreateMap<NurseProfile, NurseProfileResponseDto>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email));


            CreateMap<CreatePatientDTO, Patient>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.Ignore()) // handled separately
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => 0)) // default for now
                .ForMember(dest => dest.IsNeedNurse, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.NurseAssignment, opt => opt.Ignore()).ReverseMap();
            CreateMap<Patient, PatientResponseDTO>()
                .ForMember(dest => dest.NurseName,
                    opt => opt.MapFrom(src => src.HomeNurse != null ? src.HomeNurse.FullName : null))
                .ForMember(dest => dest.NurseId,
                    opt => opt.MapFrom(src => src.HomeNurseId))
                .ForMember(dest => dest.RelativeName,
                    opt => opt.MapFrom(src => src.Relative != null ? src.Relative.FullName : null))
                .ForMember(dest => dest.RelativeId,
                    opt => opt.MapFrom(src => src.RelativeId ?? Guid.Empty))
                .ForMember(dest => dest.PhysicalConditionDisplay,
                    opt => opt.MapFrom(src => src.PhysicalCondition.GetFlagsDisplayNames()))
                .ReverseMap();




            CreateMap<CreateVitalsDTO, Vital>().ReverseMap();
            CreateMap<VitalUpdateDTO, Vital>();
            CreateMap<Vital, VitalResponseDTO>();

            CreateMap<FoodLogCreateDTO, FoodLog>().ReverseMap();
            CreateMap<FoodLogUpdateDTO, FoodLog>().ReverseMap();
            CreateMap<FoodLog, FoodLogResponseDTO>().ReverseMap();

            CreateMap<MedicationLogCreateDTO, MedicationLog>().ReverseMap();
            CreateMap<MedicationLogUpdateDTO, MedicationLog>().ReverseMap();
            CreateMap<MedicationLog, MedicationLogResponseDTO>().ReverseMap();

            CreateMap<NurseRequest, NurseRequestResponseDTO>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName))
                .ForMember(dest => dest.RequestedBy, opt => opt.MapFrom(src => src.RequestedBy.FullName))
                .ForMember(dest => dest.PatientAge, opt => opt.MapFrom(src => src.Patient.Age))
                .ForMember(dest => dest.MedicalCondition, opt => opt.MapFrom(src => src.Patient.PhysicalCondition));

            CreateMap<NurseRequestDTO, NurseRequest>().ReverseMap();

            CreateMap<NurseAssignment, NurseAssignmentResponseDTO>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName))
                .ForMember(dest => dest.NurseName, opt => opt.MapFrom(src => src.Nurse.User.FullName))
                .ForMember(dest => dest.RequestedBy, opt => opt.MapFrom(src => src.Request.RequestedBy.FullName));

            CreateMap<CareTypeRate, CareTypeRateResponseDTO>().ReverseMap();
            CreateMap<CareTypeRateCreateDTO, CareTypeRate>().ReverseMap();
            CreateMap<CareTypeRateUpdateDTO, CareTypeRate>().ReverseMap();

        }
    }
}
