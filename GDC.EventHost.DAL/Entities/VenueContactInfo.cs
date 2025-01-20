using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDC.EventHost.DAL.Entities
{
    public class VenueContactInfo
    {
        [Required]
        public Guid VenueId { get; set; }
        public required Venue Venue { get; set; }

        [Required]
        [ForeignKey("ContactInfoId")]
        public Guid ContactInfoId { get; set; }
        public required ContactInfo ContactInfo { get; set; }

        [Required]
        public required bool IsPrimary { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }
    }
}
