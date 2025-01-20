using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.Shared.Seat;
using GDC.EventHost.Shared.SeatPosition;
using GDC.EventHost.Shared.Ticket;

namespace GDC.EventHost.API.Profiles
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketDto>();

            CreateMap<TicketDto, Ticket>();

            CreateMap<Ticket, TicketDetailDto>()
                .ForMember(
                    dest => dest.TicketStatusName,
                    opt => opt.MapFrom(src => src.TicketStatusId))
                .ForMember(
                    dest => dest.PerformanceTitle,
                    opt => opt.MapFrom(src => src.Performance.Event.Title))
                .ForMember(
                    dest => dest.PerformanceDate,
                    opt => opt.MapFrom(src => src.Performance.Date))
                .ForMember(
                    dest => dest.VenueName,
                    opt => opt.MapFrom(src => src.Performance.Venue.Name));

            CreateMap<Seat, SeatDisplayDto>()
                .ForMember(
                    dest => dest.SeatTypeName,
                    opt => opt.MapFrom(src => src.SeatTypeId));

            CreateMap<SeatPosition, SeatPositionDisplayDto>()
                .ForMember(
                    dest => dest.SeatPositionTypeName,
                    opt => opt.MapFrom(src => src.SeatPositionTypeId));

            CreateMap<TicketForUpdateDto, Ticket>();

            CreateMap<Ticket, TicketForUpdateDto>();

            CreateMap<TicketDetailDto, Ticket>();
        }
    }
}
