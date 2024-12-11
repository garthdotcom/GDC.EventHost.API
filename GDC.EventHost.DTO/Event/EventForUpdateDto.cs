using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Event
{
    public class EventForUpdateDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "You should enter a title.")]
        [MaxLength(150, ErrorMessage = "The title should not be longer than 150 characters.")]
        public required string Title { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "You should enter a description.")]
        [MaxLength(250, ErrorMessage = "The description should not be longer than 250 characters.")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Long Description")]
        [MaxLength(1500, ErrorMessage = "The long description should not be longer than 1500 characters.")]
        public string LongDescription { get; set; } = string.Empty;

        [Display(Name = "Status")]
        [Required(ErrorMessage = "You should enter a Status.")]
        public required StatusEnum StatusId { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Series")]
        [RegularExpression(@"^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$",
            ErrorMessage = "If specified, the Series Id must be a valid Guid.")]
        public Guid? SeriesId { get; set; }
    }
}