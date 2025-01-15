using AutoMapper;
using GDC.EventHost.Shared.Performance;

namespace GDC.EventHost.API.Profiles
{
    public class PerformanceProfile : Profile
    {
        public PerformanceProfile()
        {
            CreateMap<Entities.Performance, PerformanceDetailDto>()
                .ForMember(
                    dest => dest.PerformanceTypeName,
                    opt => opt.MapFrom(src => src.PerformanceType.Name))
                .ForMember(
                    dest => dest.EventTitle,
                    opt => opt.MapFrom(src => src.Event.Title))
                .ForMember(
                    dest => dest.StatusValue,
                    opt => opt.MapFrom(src => src.StatusId));
            CreateMap<PerformanceDetailDto, Entities.Performance>();

            CreateMap<Entities.Performance, PerformanceDto>();
            CreateMap<PerformanceDto, Entities.Performance>();

            CreateMap<PerformanceForUpdateDto, Entities.Performance>();
            CreateMap<Entities.Performance, PerformanceForUpdateDto>();
        }
    }
}
    