using static GDC.EventHost.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDC.EventHost.DAL.Entities
{
    public class Seat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("SeatTypeId")]
        public SeatTypeEnum SeatTypeId { get; set; }
        public virtual SeatType SeatType { get; set; }

        [Required]
        [MaxLength(10)]
        public string DisplayValue { get; set; }

        [Required]
        public int OrdinalValue { get; set; }

        [Required]
        public Guid ParentId { get; set; }

        public SeatPosition? Parent { get; set; }

        public List<Ticket> Tickets { get; set; } = [];
    }
}
