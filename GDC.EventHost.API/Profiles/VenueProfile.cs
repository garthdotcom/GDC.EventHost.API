using AutoMapper;
using GDC.EventHost.Shared.Venue;

namespace GDC.EventHost.API.Profiles
{
    public class VenueProfile : Profile
    {
        public VenueProfile()
        {
            CreateMap<Entities.Venue, VenueDetailDto>()
                .ForMember(
                    dest => dest.StatusValue,
                    opt => opt.MapFrom(src => src.StatusId));
            CreateMap<VenueDetailDto, Entities.Venue>();

            CreateMap<Entities.Venue, VenueDto>();
            CreateMap<VenueDto, Entities.Venue>();

            CreateMap<VenueForUpdateDto, Entities.Venue>();
            CreateMap<Entities.Venue, VenueForUpdateDto>();
        }
    }
}
