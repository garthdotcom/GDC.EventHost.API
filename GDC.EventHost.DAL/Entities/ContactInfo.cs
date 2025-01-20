using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDC.EventHost.DAL.Entities
{
    public class ContactInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Value { get; set; }

        [Required]
        [ForeignKey("ContactInfoTypeId")]
        public Guid ContactInfoTypeId { get; set; }
        public required ContactInfoType ContactInfoType { get; set; }

        public List<VenueContactInfo> VenueContactInfos { get; set; } = [];

        public List<PerformanceContactInfo> PerformanceContactInfo { get; set; } = [];
    }
}
