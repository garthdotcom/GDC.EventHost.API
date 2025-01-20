using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.Shared.Seat;

namespace GDC.EventHost.API.Profiles
{
    public class SeatProfile : Profile
    {
        public SeatProfile()
        {
            CreateMap<Seat, SeatDto>();

            CreateMap<Seat, SeatDisplayDto>()
                .ForMember(
                    dest => dest.SeatTypeName,
                    opt => opt.MapFrom(src => src.SeatTypeId));

            CreateMap<SeatForUpdateDto, Seat>();

            CreateMap<Seat, SeatForUpdateDto>();
        }
    }
}