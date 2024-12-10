using GDC.EventHost.DTO.Ticket;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Event
{
    public class EventDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Date")]
        public required DateTime Date { get; set; }

        [Display(Name = "Event Type")]
        public required Guid EventTypeId { get; set; }

        [Display(Name = "Status")]
        public required StatusEnum StatusId { get; set; }

        [Display(Name = "Event Summary")]
        public Guid? EventSummaryId { get; set; }

        [Display(Name = "Venue")]
        public Guid? VenueId { get; set; }

        [Display(Name = "Layout")]
        public Guid? LayoutId { get; set; }

        //****************

        public List<TicketDto> Tickets { get; set; } = [];
    }
}