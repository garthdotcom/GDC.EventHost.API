using System.ComponentModel.DataAnnotations;

namespace GDC.EventHost.DTO.EventType
{
    public class EventTypeDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public required string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
    }
}