using GDC.EventHost.DTO.EventAsset;
using GDC.EventHost.DTO.Performance;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Event
{
    public class EventDto
    {
        public Guid Id { get; set; }

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

        [Display(Name = "Status Id")]
        public StatusEnum StatusId { get; set; } = StatusEnum.Pending;

        [Display(Name = "Series Id")]
        public Guid? SeriesId { get; set; }

        public List<PerformanceDto> Performances { get; set; } = [];

        public List<EventAssetDto> EventAssets { get; set; } = [];
    }
}