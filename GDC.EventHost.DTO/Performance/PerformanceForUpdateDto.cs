using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Performance
{
    public class PerformanceForUpdateDto
    {
        [Display(Name = "Date")]
        [Required(ErrorMessage = "You should enter a Performance Date.")]
        public required DateTime Date { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Performance Type Id")]
        [Required(ErrorMessage = "You should enter a Performance Type Id.")]
        [RegularExpression(@"^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$",
            ErrorMessage = "The Performance Type Id must be a valid Guid.")]
        public required Guid PerformanceTypeId { get; set; }

        [Display(Name = "Event Id")]
        [Required(ErrorMessage = "You should enter an Event Id.")]
        [RegularExpression(@"^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$",
            ErrorMessage = "The Event Id must be a valid Guid.")]
        public required Guid EventId { get; set; }
    }
}