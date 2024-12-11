using static GDC.EventHost.DTO.Enums;

namespace GDC.EventHost.DTO.SeatingPlan
{
    public class SeatingPlanDto
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public StatusEnum StatusId { get; set; }

        public Guid VenueId { get; set; }
    }
}