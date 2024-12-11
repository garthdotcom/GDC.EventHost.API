using GDC.EventHost.DTO.SeatPosition;
using GDC.EventHost.DTO.Ticket;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Seat
{
    public class SeatDetailDto
    {
        public Guid Id { get; set; } 

        public SeatTypeEnum SeatTypeId { get; set; }

        public string SeatTypeName { get; set; } = string.Empty;

        public string DisplayValue { get; set; } = string.Empty;

        public int OrdinalValue { get; set; }

        public Guid ParentId { get; set; } 

        public SeatPositionDetailDto? SeatPositionParent { get; set; }

        public IEnumerable<TicketDto> Tickets { get; set; } = [];
    }
}