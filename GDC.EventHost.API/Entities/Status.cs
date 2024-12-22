using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.API.Entities
{
    public class Status
    {
        [Key]
        public StatusEnum Id { get; set; }

        [Required]
        [MaxLength(150)]
        public required string Name { get; set; }

        //public List<Series> Series { get; set; } = [];

        //public List<Event> Events { get; set; } = [];
        
        //public List<Performance> Performances { get; set; } = [];
    }
}
