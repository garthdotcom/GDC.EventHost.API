using System;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Series
{
    public class SeriesForUpdateDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "You should enter a name.")]
        [MaxLength(150, ErrorMessage = "The name should not be longer than 150 characters.")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "You should enter a description.")]
        [MaxLength(250, ErrorMessage = "The description should not be longer than 250 characters.")]
        public string Description { get; set; }

        [Display(Name = "Long Description")]
        [MaxLength(1500, ErrorMessage = "The long description should not be longer than 1500 characters.")]
        public string LongDescription { get; set; }

        [Display(Name = "Start Date")]
        [StartDateMustBeFuture]
        [Required(ErrorMessage = "You should enter a start date.")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [EndDateMustBeLaterThanStartDate]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "You should enter a status.")]
        public StatusEnum StatusId { get; set; }
    }
}