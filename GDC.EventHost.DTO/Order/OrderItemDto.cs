﻿using GDC.EventHost.DTO.Ticket;

namespace GDC.EventHost.DTO.Order
{
    public class OrderItemDto
    {
        public required Guid Id { get; set; }

        public required TicketDetailDto Ticket { get; set; }

        public required Guid OrderId { get; set; }
    }
}