using GDC.EventHost.DTO.Ticket;

namespace GDC.EventHost.DTO.ShoppingCart
{
    public class ShoppingCartItemForUpdateDto
    {
        public required Guid Id { get; set; }

        public required TicketDto Ticket { get; set; }

        public required Guid ShoppingCartId { get; set; }
    }
}
