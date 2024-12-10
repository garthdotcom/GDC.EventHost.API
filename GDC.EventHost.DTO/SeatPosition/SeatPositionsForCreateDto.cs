using System;
using System.Collections.Generic;
using System.Text;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.SeatPosition
{
    public class SeatPositionsForCreateDto
    {
        public int Level { get; set; }

        public int Number { get; set; }

        public SeatPositionTypeEnum SeatPositionType { get; set; } 
    }
}
