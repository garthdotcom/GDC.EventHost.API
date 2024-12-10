using GDC.EventHost.DTO.Asset;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Event
{
    public class EventDetailDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Date")]
        public required DateTime Date { get; set; }

        [Display(Name = "Event Type")]
        public required Guid EventTypeId { get; set; }

        [Display(Name = "Event Type Name")]
        public string EventTypeName { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public required StatusEnum StatusId { get; set; }

        [Display(Name = "Status Value")]
        public string StatusValue { get; set; } = string.Empty;

        [Display(Name = "Event Summary")]
        public Guid? EventSummaryId { get; set; }

        [Display(Name = "Event Title")]
        public required string EventTitle { get; set; }

        [Display(Name = "Venue")]
        public Guid? VenueId { get; set; }

        [Display(Name = "Venue Name")]
        public string VenueName { get; set; } = string.Empty;

        [Display(Name = "Layout")]
        public Guid? LayoutId { get; set; }

        [Display(Name = "Layout Name")]
        public string LayoutName { get; set; } = string.Empty;

        //[Display(Name = "Tickets Exist")]
        //public bool TicketsExist { get; set; }

        public EventTicketCount? TicketCount { get; set; }

        public List<EventAssetDto> EventAssets { get; set; } = [];
    }
}