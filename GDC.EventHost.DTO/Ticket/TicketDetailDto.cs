﻿using GDC.EventHost.DTO.Seat;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Ticket
{
    public class TicketDetailDto
    {
        public Guid Id { get; set; }

        public required string Number { get; set; }

        public decimal Price { get; set; } = decimal.Zero;

        public TicketStatusEnum TicketStatusId { get; set; }

        public string TicketStatusName { get; set; } = string.Empty;

        public Guid PerformanceId { get; set; }

        public string PerformanceTitle { get; set; } = string.Empty;

        public DateTime PerformanceDate { get; set; }

        public string VenueName { get; set; } = string.Empty;

        public Guid SeatId { get; set; }
        
        public SeatDisplayDto Seat { get; set; } 
    } 
}