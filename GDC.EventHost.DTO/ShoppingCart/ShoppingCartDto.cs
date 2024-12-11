namespace GDC.EventHost.DTO.ShoppingCart
{
    public class ShoppingCartDto
    {
        public Guid Id { get; set; }

        public Guid MemberId { get; set; }

        public List<ShoppingCartItemDto> ShoppingCartItems { get; set; } = [];
    }
}