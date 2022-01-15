using AutoMapper;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.WEB.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, CreateUserRequestDTO>()
                .ReverseMap();

            CreateMap<User, EditUserReauestDTO>()
                .ReverseMap();

            CreateMap<User, UserResponseDTO>()
                .ReverseMap();
        }


    }
}
