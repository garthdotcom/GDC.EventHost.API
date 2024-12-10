using GDC.EventHost.DTO.Event;
using System.ComponentModel.DataAnnotations;

namespace GDC.EventHost.DTO.EventType
{
    public class EventTypeDetailDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public required string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        public List<EventDetailDto> Events { get; set; } = [];
    }
}