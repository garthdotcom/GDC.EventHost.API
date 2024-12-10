using System;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Venue
{
    public class VenueForUpdateDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "You should enter a Name.")]
        [MaxLength(150, ErrorMessage = "The Name should not be longer than 150 characters.")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "You should enter a Description.")]
        [MaxLength(250, ErrorMessage = "The Description should not be longer than 250 characters.")]
        public string Description { get; set; }

        [Display(Name = "Long Description")]
        [MaxLength(1500, ErrorMessage = "The Long Description should not be longer than 1500 characters.")]
        public string LongDescription { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "You should enter a Status.")]
        public StatusEnum StatusId { get; set; }
    }
}