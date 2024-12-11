using GDC.EventHost.DTO.Ticket;

namespace GDC.EventHost.DTO.ShoppingCart
{
    public class ShoppingCartItemDto
    {
        public required Guid Id { get; set; }

        public required TicketDetailDto Ticket { get; set; }

        public required Guid ShoppingCartId { get; set; }
    }
}