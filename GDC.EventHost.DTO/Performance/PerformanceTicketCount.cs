namespace GDC.EventHost.DTO.Performance
{
    public class PerformanceTicketCount
    {
        public Guid PerformanceId { get; set; }
        public int TotalTickets { get; set; }
        public int RemainingTickets { get; set; }
    }
}
