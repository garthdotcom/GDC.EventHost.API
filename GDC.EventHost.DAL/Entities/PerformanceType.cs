using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GDC.EventHost.DAL.Entities
{
    public class PerformanceType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public required string Name { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }

        public List<Performance> Performances { get; set; } = [];
    }
}
