using GDC.EventHost.DTO.Ticket;
using System;

namespace GDC.EventHost.DTO.ShoppingCart
{
    public class ShoppingCartItemForUpdateDto
    {
        public Guid Id { get; set; }

        public TicketDto Ticket { get; set; }

        public Guid ShoppingCartId { get; set; }
    }
}
