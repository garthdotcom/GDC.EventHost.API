using AutoMapper;
using GDC.EventHost.DAL.Entities;
using GDC.EventHost.Shared.SeatingPlan;

namespace GDC.EventHost.API.Profiles
{
    public class SeatingPlanProfile : Profile
    {
        public SeatingPlanProfile()
        {
            CreateMap<SeatingPlan, SeatingPlanDto>();

            CreateMap<SeatingPlan, SeatingPlanDetailDto>()
                .ForMember(
                    dest => dest.VenueName,
                    opt => opt.MapFrom(src => src.Venue.Name))
                .ForMember(
                    dest => dest.StatusValue,
                    opt => opt.MapFrom(src => src.StatusId));

            CreateMap<SeatingPlanForUpdateDto, SeatingPlan>();

            CreateMap<SeatingPlan, SeatingPlanForUpdateDto>();
        }
    }
}
