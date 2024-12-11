using GDC.EventHost.DTO.Performance;
using System.ComponentModel.DataAnnotations;

namespace GDC.EventHost.DTO.PerformanceType
{
    public class PerformanceTypeDetailDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public required string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        public List<PerformanceDetailDto> Performances { get; set; } = [];
    }
}