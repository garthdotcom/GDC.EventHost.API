using GDC.EventHost.DTO.Performance;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.API.Entities
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }

        [MaxLength(1500)]
        public string? LongDescription { get; set; }

        [Required]
        public StatusEnum StatusId { get; set; }

        public Guid? SeriesId { get; set; }

        public List<Performance> Performances { get; set; } = [];

        public Event(string title)
        {
            Title = title;
        }
    }
}
