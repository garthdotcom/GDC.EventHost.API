using AutoMapper;
using GDC.EventHost.DTO.Event;

namespace GDC.EventHost.API.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<Entities.Event, EventWithoutPerformancesDto>();
            CreateMap<Entities.Event, EventDto>();
        }
    }
}
