﻿using GDC.EventHost.DTO.Asset;
using GDC.EventHost.DTO.EventAsset;
using GDC.EventHost.DTO.Performance;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Event
{
    public class EventDetailDto
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
        public string? Description { get; set; }

        [Display(Name = "Long Description")]
        public string? LongDescription { get; set; }

        [Display(Name = "Status Id")]
        public StatusEnum StatusId { get; set; } = StatusEnum.Pending;

        [Display(Name = "Status Value")]
        public string StatusValue { get; set; } = StatusEnum.Pending.ToString();

        [Display(Name = "Series Id")]
        public Guid? SeriesId { get; set; }

        [Display(Name = "Series Name")]
        public string SeriesName { get; set; } = string.Empty;

        public List<PerformanceDetailDto> Performances { get; set; } = [];

        public List<EventAssetDto> EventAssets { get; set; } = [];
    }
}