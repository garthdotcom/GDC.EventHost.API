using static GDC.EventHost.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDC.EventHost.DAL.Entities
{
    public class SeatPosition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("SeatPositionId")]
        public SeatPositionTypeEnum SeatPositionTypeId { get; set; }
        public virtual SeatPositionType SeatPositionType { get; set; }

        [Required]
        [MaxLength(10)]
        public string DisplayValue { get; set; }

        [Required]
        public int OrdinalValue { get; set; }

        [Required]
        public int Level { get; set; }

        [Required]
        public Guid SeatingPlanId { get; set; }

        public Guid? ParentId { get; set; }

        public SeatPosition? Parent { get; set; }

        public List<SeatPosition> Children { get; set; } = [];

        public List<Seat> Seats { get; set; } = [];
    }
}
