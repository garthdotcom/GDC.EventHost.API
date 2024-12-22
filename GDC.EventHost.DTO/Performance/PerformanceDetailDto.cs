using GDC.EventHost.DTO.PerformanceAsset;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Performance
{
    public class PerformanceDetailDto
    {
        public required Guid Id { get; set; }

        [Display(Name = "Date")]
        public required DateTime Date { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Performance Type Id")]
        public required Guid PerformanceTypeId { get; set; }

        [Display(Name = "Performance Type Name")]
        public string PerformanceTypeName { get; set; } = string.Empty;

        [Display(Name = "Event Id")]
        public required Guid EventId { get; set; }

        public PerformanceTicketCount? TicketCount { get; set; }

        public List<PerformanceAssetDto> PerformanceAssets { get; set; } = [];
    }
}