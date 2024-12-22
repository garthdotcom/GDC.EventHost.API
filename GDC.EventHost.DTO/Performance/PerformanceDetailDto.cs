using GDC.EventHost.DTO.PerformanceAsset;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Performance
{
    public class PerformanceDetailDto
    {
        public required Guid Id { get; set; }

        [Display(Name = "Date")]
        public required DateTime Date { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Performance Type Id")]
        public required Guid PerformanceTypeId { get; set; }

        [Display(Name = "Performance Type Name")]
        public string PerformanceTypeName { get; set; } = string.Empty;

        [Display(Name = "Status Id")]
        public StatusEnum StatusId { get; set; } = StatusEnum.Pending;

        [Display(Name = "Status Value")]
        public string StatusValue { get; set; } = string.Empty;

        [Display(Name = "Event Id")]
        public required Guid EventId { get; set; }

        [Display(Name = "Event Title")]
        public string EventTitle { get; set; } = string.Empty;

        [Display(Name = "Venue Id")]
        public Guid? VenueId { get; set; }

        [Display(Name = "Venue Name")]
        public string VenueName { get; set; } = string.Empty;

        [Display(Name = "SeatingPlan Id")]
        public Guid? SeatingPlanId { get; set; }

        [Display(Name = "SeatingPlan Name")]
        public string SeatingPlanName { get; set; } = string.Empty;

        public PerformanceTicketCount? TicketCount { get; set; }

        public List<PerformanceAssetDto> PerformanceAssets { get; set; } = [];
    }
}