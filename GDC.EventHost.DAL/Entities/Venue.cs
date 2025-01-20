using static GDC.EventHost.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDC.EventHost.DAL.Entities
{
    public class Venue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public required string Name { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }

        [MaxLength(1500)]
        public string? LongDescription { get; set; }

        [Required]
        [ForeignKey("StatusId")]
        public required Status Status { get; set; }
        public StatusEnum StatusId { get; set; }

        public List<Performance> Performances { get; set; } = [];

        public List<SeatingPlan> SeatingPlans { get; set; } = [];

        public List<VenueContactInfo> VenueContactInfos { get; set; } = [];

        public List<VenueAsset> VenueAssets { get; set; } = [];
    }
}
