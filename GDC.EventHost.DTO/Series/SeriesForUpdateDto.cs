using GDC.EventHost.DTO.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Series
{
    public class SeriesForUpdateDto
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

        [Display(Name = "Start Date")]
        [DateIsFutureOrNull]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DateIsFutureOrNull]
        [DateRangeIsValid("StartDate")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Status Id")]
        [Required(ErrorMessage = "You should enter a Status Id.")]
        [StatusIsValid]
        public required StatusEnum StatusId { get; set; }
    }
}