using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.Shared.Series;

namespace GDC.EventHost.API.Profiles
{
    public class SeriesProfile : Profile
    {
        public SeriesProfile()
        {
            CreateMap<Series, SeriesDetailDto>()
                .ForMember(
                    dest => dest.StatusValue,
                    opt => opt.MapFrom(src => src.StatusId));
            CreateMap<SeriesDetailDto, Series>();

            CreateMap<Series, SeriesDto>();
            CreateMap<SeriesDto, Series>();

            CreateMap<SeriesForUpdateDto, Series>();
            CreateMap<Series, SeriesForUpdateDto>();
        }
    }
}
