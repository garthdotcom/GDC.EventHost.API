using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDC.EventHost.DAL.Entities
{
    public class ShoppingCartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public required Ticket Ticket { get; set; }

        [Required]
        public required Guid ShoppingCartId { get; set; }
    }
}
