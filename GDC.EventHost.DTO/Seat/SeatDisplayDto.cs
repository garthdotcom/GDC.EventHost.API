using GDC.EventHost.DTO.SeatPosition;
using System;
using System.Collections.Generic;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Seat
{
    public class SeatDisplayDto 
    {
        public List<SeatPositionDisplayDto> SeatPositions { get; set; } 

        public Guid Id { get; set; }

        public SeatTypeEnum SeatTypeId { get; set; }

        public string SeatTypeName { get; set; }

        public string DisplayValue { get; set; }

        public int OrdinalValue { get; set; }

        public Guid ParentId { get; set; }

        public Guid LayoutId { get; set; }

        public Guid VenueId { get; set; }

        public string VenueName { get; set; }
    }
}
