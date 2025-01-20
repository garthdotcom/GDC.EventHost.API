using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.Shared.PerformanceType;

namespace GDC.PerformanceHost.API.Profiles
{
    public class PerformanceTypeProfile : Profile
    {
        public PerformanceTypeProfile()
        {
            CreateMap<PerformanceType, PerformanceTypeDto>();
            CreateMap<PerformanceType, PerformanceTypeDetailDto>();
            CreateMap<PerformanceTypeForUpdateDto, PerformanceType>();
            CreateMap<PerformanceType, PerformanceTypeForUpdateDto>();
        }
    }
}
