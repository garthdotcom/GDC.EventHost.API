﻿namespace GDC.EventHost.DTO.Performance
{
    public class PerformanceTicketCount
    {
        public required Guid PerformanceId { get; set; }
        public required int TotalTickets { get; set; }
        public required int RemainingTickets { get; set; }
    }
}
