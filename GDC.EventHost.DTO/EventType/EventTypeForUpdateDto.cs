﻿using System;
using System.ComponentModel.DataAnnotations;

namespace GDC.EventHost.DTO.EventType
{
    public class EventTypeForUpdateDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "You should enter a Name.")]
        [MaxLength(150, ErrorMessage = "The Event Type Name should not be longer than 150 characters.")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [MaxLength(250, ErrorMessage = "The Event Type Description should not be longer than 250 characters.")]
        public string Description { get; set; }
    }
}