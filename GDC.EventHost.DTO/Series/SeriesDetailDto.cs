using GDC.EventHost.DTO.Event;
using GDC.EventHost.DTO.SeriesAsset;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Series
{
    public class SeriesDetailDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Title")]
        public required string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Long Description")]
        public string LongDescription { get; set; } = string.Empty;

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Status Id")]
        public StatusEnum StatusId { get; set; } = StatusEnum.Pending;

        [Display(Name = "Status Value")]
        public string StatusValue { get; set; } = string.Empty;

        public List<EventDto> Events { get; set; } = [];

        public List<SeriesAssetDto> SeriesAssets { get; set; } = [];
    }
}