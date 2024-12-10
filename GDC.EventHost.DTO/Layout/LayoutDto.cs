using System;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Layout
{
    public class LayoutDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public StatusEnum StatusId { get; set; }

        public Guid VenueId { get; set; }
    }
}