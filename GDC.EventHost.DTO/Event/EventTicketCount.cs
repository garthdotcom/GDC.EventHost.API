namespace GDC.EventHost.DTO.Event
{
    public class EventTicketCount
    {
        public Guid EventId { get; set; }
        public int TotalTickets { get; set; }
        public int RemainingTickets { get; set; }
    }
} 
