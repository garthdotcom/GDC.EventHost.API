using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.SeatPosition
{
    public class SeatPositionDisplayDto
    {
        public int Level { get; set; }

        public SeatPositionTypeEnum SeatPositionTypeId { get; set; }

        public string SeatPositionTypeName { get; set; } = string.Empty;

        public int OrdinalValue { get; set; }

        public string DisplayValue { get; set; } = string.Empty;
    }
}
