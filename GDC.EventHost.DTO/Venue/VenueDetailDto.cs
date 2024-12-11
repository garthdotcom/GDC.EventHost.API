using GDC.EventHost.DTO.Event;
using GDC.EventHost.DTO.SeatingPlan;
using GDC.EventHost.DTO.VenueAsset;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Venue
{
    public class VenueDetailDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public required string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Long Description")]
        public string LongDescription { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public StatusEnum StatusId { get; set; } = StatusEnum.Pending;

        [Display(Name = "Status Value")]
        public string StatusValue { get; set; } = string.Empty;

        public List<EventDetailDto> Events { get; set; } = [];

        public List<VenueAssetDto> VenueAssets { get; set; } = [];

        public List<SeatingPlanDto> SeatingPlans { get; set; } = [];
    }
}