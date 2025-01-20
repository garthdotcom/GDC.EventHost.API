using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDC.EventHost.DAL.Entities
{
    public class ContactInfoType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Value { get; set; }

        public List<ContactInfo> ContactInfos { get; set; } = [];
    }
}
