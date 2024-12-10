using GDC.EventHost.DTO.Ticket;
using System;

namespace GDC.EventHost.DTO.Order
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }

        public TicketDetailDto Ticket { get; set; }

        public Guid OrderId { get; set; }
    }
}