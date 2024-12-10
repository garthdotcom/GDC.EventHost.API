using GDC.EventHost.DTO.Seat;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Ticket
{
    public class TicketDetailDto
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public decimal Price { get; set; }

        public TicketStatusEnum TicketStatusId { get; set; }

        public string TicketStatusName { get; set; }

        public Guid EventId { get; set; }

        public string EventTitle { get; set; }

        public DateTime EventDate { get; set; }

        public string VenueName { get; set; }

        public Guid SeatId { get; set; }
        
        public SeatDisplayDto Seat { get; set; } 
    } 
}