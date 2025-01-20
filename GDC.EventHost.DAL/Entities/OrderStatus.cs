using static GDC.EventHost.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace GDC.EventHost.DAL.Entities
{
    public class OrderStatus
    {
        [Key]
        public OrderStatusEnum Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }
    }
}
