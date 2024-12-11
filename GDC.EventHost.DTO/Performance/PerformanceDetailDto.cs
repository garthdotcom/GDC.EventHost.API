using GDC.EventHost.DTO.PerformanceAsset;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Performance
{
    public class PerformanceDetailDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Date")]
        public required DateTime Date { get; set; }

        [Display(Name = "Performance Type")]
        public required Guid PerformanceTypeId { get; set; }

        [Display(Name = "Performance Type Name")]
        public string PerformanceTypeName { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public required StatusEnum StatusId { get; set; }

        [Display(Name = "Status Value")]
        public string StatusValue { get; set; } = string.Empty;

        [Display(Name = "Event")]
        public Guid? EventId { get; set; }

        [Display(Name = "Performance Title")]
        public required string PerformanceTitle { get; set; }

        [Display(Name = "Venue")]
        public Guid? VenueId { get; set; }

        [Display(Name = "Venue Name")]
        public string VenueName { get; set; } = string.Empty;

        [Display(Name = "SeatingPlanId")]
        public Guid? SeatingPlanId { get; set; }

        [Display(Name = "SeatingPlan Name")]
        public string SeatingPlanName { get; set; } = string.Empty;

        public PerformanceTicketCount? TicketCount { get; set; }

        public List<PerformanceAssetDto> PerformanceAssets { get; set; } = [];
    }
}