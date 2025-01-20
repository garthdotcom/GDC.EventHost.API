using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.Shared.Enums;

namespace GDC.EventHost.DAL.Entities
{
    public class AssetType
    {
        [Key]
        public AssetTypeEnum Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }
    }
}