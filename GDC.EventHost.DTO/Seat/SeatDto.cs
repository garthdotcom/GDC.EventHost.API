using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Seat
{
    public class SeatDto
    {
        public Guid Id { get; set; }

        public SeatTypeEnum SeatType { get; set; }

        public string DisplayValue { get; set; } = string.Empty;

        public int OrdinalValue { get; set; }

        public Guid SeatPositionParentId { get; set; }
    }
}