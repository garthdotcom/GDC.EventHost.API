using System;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.SeatPosition
{
    public class SeatPositionDto
    {
        public Guid Id { get; set; }

        // section, row
        public SeatPositionTypeEnum SeatPositionTypeId { get; set; }

        public string DisplayValue { get; set; }

        public int OrdinalValue { get; set; }

        public int Level { get; set; }
    }
}