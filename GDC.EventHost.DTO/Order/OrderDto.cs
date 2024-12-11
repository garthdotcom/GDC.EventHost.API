using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }

        public Guid MemberId { get; set; }

        public OrderStatusEnum OrderStatusId { get; set; }

        public string OrderStatusValue { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public List<OrderItemDto> OrderItems { get; set; } = [];
    }
}