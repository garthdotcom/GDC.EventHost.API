using System;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Event
{
    public class EventForUpdateDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "You should enter an Event Date.")]
        public DateTime Date { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "You should enter an Event Type.")]
        [RegularExpression(@"^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$",
            ErrorMessage = "The Event Type Id must be a valid Guid.")]
        public Guid EventTypeId { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "You should enter a Status.")]
        public StatusEnum StatusId { get; set; }

        [Display(Name = "Event")]
        [Required(ErrorMessage = "You should enter an Event Summary.")]
        [RegularExpression(@"^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$",
            ErrorMessage = "The Event Summary Id must be a valid Guid.")]
        public Guid EventSummaryId { get; set; }

        [Display(Name = "Venue")]
        [RegularExpression(@"^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$",
            ErrorMessage = "If specified, the Venue Id must be a valid Guid.")]
        public Guid? VenueId { get; set; }

        [Display(Name = "Layout")]
        [RegularExpression(@"^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$",
            ErrorMessage = "If specified, the Layout Id must be a valid Guid.")]
        public Guid? LayoutId { get; set; } 
    }
}