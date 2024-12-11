using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Performance
{
    public class PerformanceForUpdateDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "You should enter an Performance Date.")]
        public required DateTime Date { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "You should enter an Performance Type.")]
        [RegularExpression(@"^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$",
            ErrorMessage = "The Event Type Id must be a valid Guid.")]
        public required Guid PerformanceTypeId { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "You should enter a Status.")]
        public required StatusEnum StatusId { get; set; }

        [Display(Name = "Event")]
        [Required(ErrorMessage = "You should enter an Event Id.")]
        [RegularExpression(@"^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$",
            ErrorMessage = "The Event Id must be a valid Guid.")]
        public required Guid EventId { get; set; }

        [Display(Name = "Venue")]
        [RegularExpression(@"^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$",
            ErrorMessage = "If specified, the Venue Id must be a valid Guid.")]
        public Guid? VenueId { get; set; }

        [Display(Name = "SeatingPlan")]
        [RegularExpression(@"^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$",
            ErrorMessage = "If specified, the Seating Plan Id must be a valid Guid.")]
        public Guid? SeatingPlanId { get; set; }
    }
}