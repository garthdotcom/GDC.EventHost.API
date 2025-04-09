using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.Shared.Performance;

namespace GDC.EventHost.API.Profiles
{
    public class PerformanceProfile : Profile
    {
        public PerformanceProfile()
        {
            CreateMap<Performance, PerformanceDto>();
            CreateMap<PerformanceDto, Performance>();

            CreateMap<Performance, PerformanceDetailDto>()
                .ForMember(
                    dest => dest.PerformanceTypeName,
                    opt => opt.MapFrom(src => src.PerformanceType.Name))
                .ForMember(
                    dest => dest.EventTitle,
                    opt => opt.MapFrom(src => src.Event.Title))
                .ForMember(
                    dest => dest.VenueName,
                    opt => opt.MapFrom(src => src.Venue.Name))
                .ForMember(
                    dest => dest.SeatingPlanName,
                    opt => opt.MapFrom(src => src.SeatingPlan.Name))
                .ForMember(
                    dest => dest.StatusValue,
                    opt => opt.MapFrom(src => src.StatusId));

            CreateMap<PerformanceDetailDto, Performance>();

            CreateMap<PerformanceForCreateDto, Performance>();
            CreateMap<Performance, PerformanceForCreateDto>();

            CreateMap<PerformanceForUpdateDto, Performance>();
            CreateMap<Performance, PerformanceForUpdateDto>();
        }
    }
}
    