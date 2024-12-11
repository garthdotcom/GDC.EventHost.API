using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Venue
{
    public class VenueForUpdateDto
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "You should enter a Name.")]
        [MaxLength(150, ErrorMessage = "The Name should not be longer than 150 characters.")]
        public required string Name { get; set; }

        [Display(Name = "Description")]
        [MaxLength(250, ErrorMessage = "The Description should not be longer than 250 characters.")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Long Description")]
        [MaxLength(1500, ErrorMessage = "The Long Description should not be longer than 1500 characters.")]
        public string LongDescription { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public StatusEnum StatusId { get; set; } = StatusEnum.Pending;
    }
}