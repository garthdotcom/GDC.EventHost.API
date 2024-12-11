using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Performance
{
    public class PerformanceDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Date")]
        public required DateTime Date { get; set; }

        [Display(Name = "Performance Type")]
        public required Guid PerformanceTypeId { get; set; }

        [Display(Name = "Status")]
        public required StatusEnum StatusId { get; set; }

        [Display(Name = "Event")]
        public Guid? EventId { get; set; }

        [Display(Name = "Venue")]
        public Guid? VenueId { get; set; }

        [Display(Name = "SeatingPlan")]
        public Guid? SeatingPlanId { get; set; }
    }
}