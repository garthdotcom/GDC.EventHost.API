using System.ComponentModel.DataAnnotations;

namespace GDC.EventHost.DTO.PerformanceType
{
    public class PerformanceTypeDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public required string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
    }
}