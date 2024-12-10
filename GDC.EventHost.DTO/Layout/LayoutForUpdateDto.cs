﻿using System;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.Layout
{
    public class LayoutForUpdateDto
    {
        public Guid Id { get; set; } 

        [Display(Name = "Name")]
        [Required(ErrorMessage = "You should enter a Name.")]
        [MaxLength(150, ErrorMessage = "The Event Type Name should not be longer than 150 characters.")]
        public string Name { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "You should enter a Status.")]
        public StatusEnum StatusId { get; set; }

        [Display(Name = "Venue")]
        [RegularExpression(@"^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$",
            ErrorMessage = "If specified, the Venue Id must be a valid Guid.")]
        public Guid VenueId { get; set; }
    }
}