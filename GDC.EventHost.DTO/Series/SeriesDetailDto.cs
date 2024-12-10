using GDC.EventHost.DTO.Asset;
using GDC.EventHost.DTO.EventSummary;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Series
{
    public class SeriesDetailDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public required string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Long Description")]
        public string LongDescription { get; set; } = string.Empty;

        [Display(Name = "Start Date")]
        public required DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Status")]
        public StatusEnum StatusId { get; set; }

        [Display(Name = "Status Value")]
        public required string StatusValue { get; set; }

        public List<EventSummaryDto> EventSummaries { get; set; } = [];

        public List<SeriesAssetDto> SeriesAssets { get; set; } = [];
    }
}