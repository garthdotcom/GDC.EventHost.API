using GDC.EventHost.DTO.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GDC.EventHost.DTO.EventType
{
    public class EventTypeDetailDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public List<EventDetailDto> Events { get; set; }
    }
}