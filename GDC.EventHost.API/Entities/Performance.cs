using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.API.Entities
{
    public class Performance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        public Guid PerformanceTypeId { get; set; }

        [Required]
        public StatusEnum StatusId { get; set; }

        public Guid? VenueId { get; set; }

        public Guid? SeatingPlanId { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }
        public Guid EventId { get; set; }

        public Performance(string title)
        {
            Title = title;
        }
    }
}
