using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.Shared.Enums;

namespace GDC.EventHost.DAL.Entities
{
    public class Series
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public required string Title { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }

        [MaxLength(1500)]
        public string? LongDescription { get; set; }

        [Required]
        [ForeignKey("StatusId")]
        public required Status Status { get; set; }
        public StatusEnum StatusId { get; set; }

        public List<Event> Events { get; set; } = [];

        public List<SeriesAsset> SeriesAssets { get; set; } = [];
    }
}
