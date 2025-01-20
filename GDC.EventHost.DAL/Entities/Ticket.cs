using static GDC.EventHost.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDC.EventHost.DAL.Entities
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(35)]
        public string? Number { get; set; }

        [Required]
        public required decimal Price { get; set; }

        [Required]
        [ForeignKey("TicketStatusId")]
        public TicketStatusEnum TicketStatusId { get; set; }
        public required virtual TicketStatus TicketStatus { get; set; }

        [Required]
        [ForeignKey("PerformanceId")]
        public Guid PerformanceId { get; set; }
        public required Performance Performance { get; set; }

        [Required]
        [ForeignKey("SeatId")]
        public Guid SeatId { get; set; }
        public required Seat Seat { get; set; }
    }
}
