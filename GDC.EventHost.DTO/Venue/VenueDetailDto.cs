using GDC.EventHost.DTO.Asset;
using GDC.EventHost.DTO.Event;
using GDC.EventHost.DTO.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Venue
{
    public class VenueDetailDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Long Description")]
        public string LongDescription { get; set; }

        [Display(Name = "Status")]
        public StatusEnum StatusId { get; set; }

        [Display(Name = "Status Value")]
        public string StatusValue { get; set; }

        public List<EventDetailDto> Events { get; set; }

        public List<VenueAssetDto> VenueAssets { get; set; }

        public List<LayoutDetailDto> Layouts { get; set; }
    }
}