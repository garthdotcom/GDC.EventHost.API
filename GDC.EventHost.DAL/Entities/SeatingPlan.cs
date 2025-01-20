using static GDC.EventHost.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDC.EventHost.DAL.Entities
{
    public class SeatingPlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [ForeignKey("StatusId")]
        public StatusEnum StatusId { get; set; }
        public virtual Status Status { get; set; }

        [Required]
        [ForeignKey("VenueId")]
        public Guid VenueId { get; set; }
        public Venue Venue { get; set; }

        public List<Performance> Performances { get; set; } = [];

        public List<SeatPosition> SeatPositions { get; set; } = [];
    }
}
