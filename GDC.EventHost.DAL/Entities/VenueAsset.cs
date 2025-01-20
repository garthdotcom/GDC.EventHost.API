using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GDC.EventHost.Shared.Enums;

namespace GDC.EventHost.DAL.Entities
{
    public class VenueAsset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("VenueId")]
        public Guid VenueId { get; set; }
        public Venue Venue { get; set; }

        [Required]
        [ForeignKey("AssetId")]
        public Guid AssetId { get; set; }
        public Asset Asset { get; set; }

        [Required]
        [ForeignKey("AssetTypeId")]
        public AssetTypeEnum AssetTypeId { get; set; }
        public virtual AssetType AssetType { get; set; }

        // when multiple assets of the same type exist for a Venue, 
        // this indicates priority
        [Required]
        public required int OrdinalValue { get; set; }
    }
}
