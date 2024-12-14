using AutoMapper;
using GDC.EventHost.DTO.Performance;

namespace GDC.EventHost.API.Profiles
{
    public class PerformanceProfile : Profile
    {
        public PerformanceProfile()
        {
            CreateMap<Entities.Performance,PerformanceDto>();
            CreateMap<PerformanceForUpdateDto, Entities.Performance>();
            CreateMap<Entities.Performance, PerformanceForUpdateDto>();
        }
    }
}
    