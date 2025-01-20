using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDC.EventHost.DAL.Entities
{
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public required string MembershipNumber { get; set; }

        public required string Username { get; set; }

        public required string FullName { get; set; }

        public required string EmailAddress { get; set; }

        public required bool IsActive { get; set; }

        public ShoppingCart? ShoppingCart { get; set; }

        public List<MemberAsset> MemberAssets { get; set; } = [];

        public List<Order> Orders { get; set; } = [];
    }
}
