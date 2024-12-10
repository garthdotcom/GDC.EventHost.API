using System;
using System.ComponentModel.DataAnnotations;
using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.EventSummary
{
    public class EventSummaryDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Long Description")]
        public string LongDescription { get; set; }

        [Display(Name = "Status")]
        public StatusEnum StatusId { get; set; }

        [Display(Name = "Series")]
        public Guid? SeriesId { get; set; }
    }
}