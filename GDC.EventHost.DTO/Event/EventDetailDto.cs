using GDC.EventHost.DTO.Asset;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Event
{
    public class EventDetailDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Event Type")]
        public Guid EventTypeId { get; set; }

        [Display(Name = "Event Type Name")]
        public string EventTypeName { get; set; } 

        [Display(Name = "Status")]
        public StatusEnum StatusId { get; set; }

        [Display(Name = "Status Value")]
        public string StatusValue { get; set; }

        [Display(Name = "Event Summary")]
        public Guid? EventSummaryId { get; set; }

        [Display(Name = "Event Title")]
        public string EventTitle { get; set; }

        [Display(Name = "Venue")]
        public Guid? VenueId { get; set; }

        [Display(Name = "Venue Name")]
        public string VenueName { get; set; }

        [Display(Name = "Layout")]
        public Guid? LayoutId { get; set; }

        [Display(Name = "Layout Name")]
        public string LayoutName { get; set; }

        //[Display(Name = "Tickets Exist")]
        //public bool TicketsExist { get; set; }

        public EventTicketCount TicketCount { get; set; } 

        public List<EventAssetDto> EventAssets { get; set; }
    }
}