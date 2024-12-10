using GDC.EventHost.DTO.Ticket;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Event
{
    public class EventDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Event Type")]
        public Guid EventTypeId { get; set; }

        [Display(Name = "Status")]
        public StatusEnum StatusId { get; set; } = StatusEnum.Pending;

        [Display(Name = "Event Summary")]
        public Guid? EventSummaryId { get; set; }

        [Display(Name = "Venue")]
        public Guid? VenueId { get; set; }

        [Display(Name = "Layout")]
        public Guid? LayoutId { get; set; }

        //****************

        public List<TicketDto> Tickets{ get; set; } = new List<TicketDto>();
    }
}