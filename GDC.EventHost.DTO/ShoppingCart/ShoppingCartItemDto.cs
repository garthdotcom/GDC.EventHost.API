using GDC.EventHost.DTO.Ticket;
using System;

namespace GDC.EventHost.DTO.ShoppingCart
{
    public class ShoppingCartItemDto
    {
        public Guid Id { get; set; }

        public TicketDetailDto Ticket { get; set; }

        public Guid ShoppingCartId { get; set; }
    }
}