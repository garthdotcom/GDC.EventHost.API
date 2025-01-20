using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.Shared.Venue;

namespace GDC.EventHost.API.Profiles
{
    public class VenueProfile : Profile
    {
        public VenueProfile()
        {
            CreateMap<Venue, VenueDetailDto>()
                .ForMember(
                    dest => dest.StatusValue,
                    opt => opt.MapFrom(src => src.StatusId));
            CreateMap<VenueDetailDto, Venue>();

            CreateMap<Venue, VenueDto>();
            CreateMap<VenueDto, Venue>();

            CreateMap<VenueForUpdateDto, Venue>();
            CreateMap<Venue, VenueForUpdateDto>();
        }
    }
}
