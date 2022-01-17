using AutoMapper;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.WEB.AutoMapperProfiles
{
    public class TimeOffProfile : Profile
    {
        public TimeOffProfile()
        {
            CreateMap<TimeOff, TimeOffRequestDTO>()
                .ReverseMap();

            CreateMap<TimeOff, TimeOffResponseDTO>()
                .ReverseMap();
        }
    }
}
