using GDC.EventHost.DTO.Event;
using GDC.EventHost.DTO.Seat;
using GDC.EventHost.DTO.SeatPosition;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Layout
{
    public class LayoutDetailDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Status")]
        public StatusEnum StatusId { get; set; }

        [Display(Name = "Status Value")]
        public string StatusValue { get; set; }

        [Display(Name = "Venue")]
        public Guid VenueId { get; set; }

        [Display(Name = "Venue Name")]
        public string VenueName { get; set; }

        public List<SeatDisplayDto> SeatsForDisplay { get; set; }
            = new List<SeatDisplayDto>();

        //public List<SeatPositionDetailDto> SeatPositions { get; set; }
        //    = new List<SeatPositionDetailDto>();

        public List<EventDetailDto> Events { get; set; }
            = new List<EventDetailDto>();
    }
}
