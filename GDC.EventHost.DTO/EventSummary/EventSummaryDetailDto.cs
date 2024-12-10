﻿using GDC.EventHost.DTO.Asset;
using GDC.EventHost.DTO.Event;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.EventSummary
{
    public class EventSummaryDetailDto
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public required string Title { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Long Description")]
        public string LongDescription { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public required StatusEnum StatusId { get; set; }

        [Display(Name = "Status Value")]
        public string StatusValue { get; set; } = string.Empty;

        [Display(Name = "Series")]
        public Guid? SeriesId { get; set; }

        [Display(Name = "Series Name")]
        public string SeriesName { get; set; } = string.Empty;

        public List<EventDetailDto> Events { get; set; } = [];

        public List<EventSummaryAssetDto> EventSummaryAssets { get; set; } = [];
    }
}