﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.API.Entities
{
    public class Performance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(150)]
        public string? Title { get; set; }

        [Required]
        [ForeignKey("PerformanceTypeId")]
        public required PerformanceType PerformanceType { get; set; }
        public Guid PerformanceTypeId { get; set; }

        [Required]
        [ForeignKey("EventId")]
        public required Event Event { get; set; }
        public Guid EventId { get; set; }

        [Required]
        [ForeignKey("StatusId")]
        public required Status Status { get; set; }
        public StatusEnum StatusId { get; set; }

        [Required]
        [ForeignKey("VenueId")]
        public required Venue Venue { get; set; }
        public Guid VenueId { get; set; }
    }
}
