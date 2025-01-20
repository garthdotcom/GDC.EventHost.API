using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDC.EventHost.DAL.Entities
{
    public class ShoppingCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public required Guid MemberId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = [];
    }
}
