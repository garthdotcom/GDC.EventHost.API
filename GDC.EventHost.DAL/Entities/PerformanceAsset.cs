using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GDC.EventHost.Shared.Enums;

namespace GDC.EventHost.DAL.Entities
{
    public class PerformanceAsset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("PerformanceId")]
        public Guid PerformanceId { get; set; }
        public required Performance Performance { get; set; }

        [Required]
        [ForeignKey("AssetId")]
        public Guid AssetId { get; set; }
        public required Asset Asset { get; set; }

        [Required]
        [ForeignKey("AssetTypeId")]
        public AssetTypeEnum AssetTypeId { get; set; }
        public required virtual AssetType AssetType { get; set; }

        // when multiple assets of the same type exist for a Performance, 
        // this indicates priority
        [Required]
        public required int OrdinalValue { get; set; }
    }
}
