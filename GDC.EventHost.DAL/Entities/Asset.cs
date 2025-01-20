using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDC.EventHost.DAL.Entities
{
    public class Asset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public required string Name { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Uri { get; set; }

        public List<SeriesAsset> SeriesAssets { get; set; } = [];

        public List<EventAsset> EventAssets { get; set; } = [];

        public List<PerformanceAsset> PerformanceAssets { get; set; } = [];

        public List<VenueAsset> VenueAssets { get; set; } = [];

        public List<MemberAsset> MemberAssets { get; set; } = [];
    }
}
