using AutoMapper;
using GDC.EventHost.DTO.Event;

namespace GDC.EventHost.API.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<Entities.Event, EventDetailDto>()
                .ForMember(
                    dest => dest.SeriesTitle,
                    opt => opt.MapFrom(src => src.Series != null ? src.Series.Title : "Series not found"))
                .ForMember(
                    dest => dest.StatusValue,
                    opt => opt.MapFrom(src => src.StatusId));
            CreateMap<EventDetailDto, Entities.Event>();

            CreateMap<Entities.Event, EventDto>();
            CreateMap<EventDto, Entities.Event>();

            CreateMap<EventForUpdateDto, Entities.Event>();
            CreateMap<Entities.Event, EventForUpdateDto>();
        }
    }
}
