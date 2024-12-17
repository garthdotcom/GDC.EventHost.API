using AutoMapper;
using GDC.EventHost.DTO.Series;

namespace GDC.EventHost.API.Profiles
{
    public class SeriesProfile : Profile
    {
        public SeriesProfile()
        {
            CreateMap<Entities.Series, SeriesWithoutEventsDto>();
            CreateMap<Entities.Series, SeriesDto>();
            CreateMap<SeriesForUpdateDto, Entities.Series>();
            CreateMap<Entities.Series, SeriesForUpdateDto>();
        }
    }
}
