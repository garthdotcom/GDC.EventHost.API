using static GDC.EventHost.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDC.EventHost.DAL.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public required Guid MemberId { get; set; }

        [Required]
        public required DateTime Date { get; set; }

        [Required]
        [ForeignKey("OrderStatusId")]
        public OrderStatusEnum OrderStatusId { get; set; }
        public required virtual OrderStatus OrderStatus { get; set; }

        public List<OrderItem> OrderItems { get; set; } = [];
    }
}
