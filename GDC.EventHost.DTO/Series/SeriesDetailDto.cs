using GDC.EventHost.DTO.Asset;
using GDC.EventHost.DTO.EventSummary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Series
{
    public class SeriesDetailDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Long Description")]
        public string LongDescription { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Status")]
        public StatusEnum StatusId { get; set; }

        [Display(Name = "Status Value")]
        public string StatusValue { get; set; } 

        public List<EventSummaryDto> EventSummaries { get; set; }

        public List<SeriesAssetDto> SeriesAssets { get; set; }
    }
}