using AutoMapper;
using GDC.EventHost.Shared.Series;

namespace GDC.EventHost.API.Profiles
{
    public class SeriesProfile : Profile
    {
        public SeriesProfile()
        {
            CreateMap<Entities.Series, SeriesDetailDto>()
                .ForMember(
                    dest => dest.StatusValue,
                    opt => opt.MapFrom(src => src.StatusId));
            CreateMap<SeriesDetailDto, Entities.Series>();

            CreateMap<Entities.Series, SeriesDto>();
            CreateMap<SeriesDto, Entities.Series>();

            CreateMap<SeriesForUpdateDto, Entities.Series>();
            CreateMap<Entities.Series, SeriesForUpdateDto>();
        }
    }
}
