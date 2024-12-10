using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Ticket
{
    public class TicketDto
    {
        public Guid Id { get; set; }

        public string Number { get; set; } = string.Empty;

        public decimal Price { get; set; } = decimal.Zero;

        public TicketStatusEnum TicketStatusId { get; set; } = TicketStatusEnum.UnSold;

        public Guid EventId { get; set; }

        public Guid SeatId { get; set; }
    }
}