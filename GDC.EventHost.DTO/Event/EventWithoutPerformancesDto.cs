using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Event
{
    public class EventWithoutPerformancesDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Title")]
        public required string Title { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Long Description")]
        public string? LongDescription { get; set; }

        [Display(Name = "Series Id")]
        public Guid? SeriesId { get; set; }
    }
}
