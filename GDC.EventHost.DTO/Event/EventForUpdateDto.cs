using GDC.EventHost.DTO.Event.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Event
{
    public class EventForUpdateDto
    {
        [Display(Name = "Title")]
        [Required(ErrorMessage = "You should enter a title.")]
        [MaxLength(150, ErrorMessage = "The title should not be longer than 150 characters.")]
        public required string Title { get; set; }

        [Display(Name = "Description")]
        [MaxLength(250, ErrorMessage = "The description should not be longer than 250 characters.")]
        public string? Description { get; set; }

        [Display(Name = "Long Description")]
        [MaxLength(1500, ErrorMessage = "The long description should not be longer than 1500 characters.")]
        public string? LongDescription { get; set; }

        [Display(Name = "Status Id")]
        public StatusEnum StatusId { get; set; } = StatusEnum.Pending;

        [Display(Name = "Start Date")]
        [StartDateMustBeFuture]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [EndDateMustBeLaterThanStartDate]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Series Id")]
        [RegularExpression(@"^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$",
            ErrorMessage = "If specified, the Series Id must be a valid Guid.")]
        public Guid? SeriesId { get; set; }
    }
}