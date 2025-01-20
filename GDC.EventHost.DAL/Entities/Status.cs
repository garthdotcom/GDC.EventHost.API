using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.Shared.Enums;

namespace GDC.EventHost.DAL.Entities
{
    public class Status
    {
        [Key]
        public StatusEnum Id { get; set; }

        [Required]
        [MaxLength(150)]
        public required string Name { get; set; }
    }
}
