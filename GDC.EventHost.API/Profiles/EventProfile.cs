using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.Shared.Event;

namespace GDC.EventHost.API.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<Event, EventDetailDto>()
                .ForMember(
                    dest => dest.SeriesTitle,
                    opt => opt.MapFrom(src => src.Series != null ? src.Series.Title : "Series not found"))
                .ForMember(
                    dest => dest.StatusValue,
                    opt => opt.MapFrom(src => src.StatusId));
            CreateMap<EventDetailDto, Event>();

            CreateMap<Event, EventDto>();
            CreateMap<EventDto, Event>();

            CreateMap<EventForUpdateDto, Event>();
            CreateMap<Event, EventForUpdateDto>();
        }
    }
}
