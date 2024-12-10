using GDC.EventHost.DTO;
using GDC.EventHost.DTO.Event;
using GDC.EventHost.DTO.Ticket;

namespace GDC.EventHost.API
{
    public class EventsDataStore
    {
        public List<EventDto> Events { get; set; }
        public List<TicketDto> Tickets { get; set; }

        public static EventsDataStore Current { get; } = new EventsDataStore();

        public EventsDataStore()
        {
            var event1Id = Guid.NewGuid();
            var event2Id = Guid.NewGuid();
            var event3Id = Guid.NewGuid();
            var event1SummaryId = Guid.NewGuid();
            var event2SummaryId = Guid.NewGuid();
            var event3SummaryId = Guid.NewGuid();
            var event1TypeId = Guid.NewGuid();
            var event2TypeId = Guid.NewGuid();
            var event3TypeId = Guid.NewGuid();
            var venue1Id = Guid.NewGuid();
            var venue2Id = Guid.NewGuid();
            var venue3Id = Guid.NewGuid();

            // init dummy data
            Tickets = new List<TicketDto>()
            {
                new TicketDto()
                {
                    Id = Guid.NewGuid(),
                    EventId = event1Id,
                    Number = "01",
                    SeatId = Guid.NewGuid(),
                    TicketStatusId = Enums.TicketStatusEnum.Sold,
                    Price = 25M
                },
                new TicketDto()
                {
                    Id = Guid.NewGuid(),
                    EventId = event1Id,
                    Number = "02",
                    SeatId = Guid.NewGuid(),
                    TicketStatusId = Enums.TicketStatusEnum.Sold,
                    Price = 25M
                },
                new TicketDto()
                {
                    Id = Guid.NewGuid(),
                    EventId = event1Id,
                    Number = "03",
                    SeatId = Guid.NewGuid(),
                    TicketStatusId = Enums.TicketStatusEnum.UnSold,
                    Price = 25M
                },
                new TicketDto()
                {
                    Id = Guid.NewGuid(),
                    EventId = event2Id,
                    Number = "01",
                    SeatId = Guid.NewGuid(),
                    TicketStatusId = Enums.TicketStatusEnum.Sold,
                    Price = 25M
                },
                new TicketDto()
                {
                    Id = Guid.NewGuid(),
                    EventId = event2Id,
                    Number = "02",
                    SeatId = Guid.NewGuid(),
                    TicketStatusId = Enums.TicketStatusEnum.Sold,
                    Price = 25M
                },
                new TicketDto()
                {
                    Id = Guid.NewGuid(),
                    EventId = event3Id,
                    Number = "01",
                    SeatId = Guid.NewGuid(),
                    TicketStatusId = Enums.TicketStatusEnum.Sold,
                    Price = 25M
                }
            };

            Events = new List<EventDto>()
            {
                new EventDto()
                {
                    Id = event1Id,
                    Date = DateTime.Now,
                    EventSummaryId = event1SummaryId,
                    EventTypeId = event1TypeId,
                    LayoutId = null,
                    StatusId = Enums.StatusEnum.Active,
                    VenueId = venue1Id,
                    Tickets = Tickets.Where(t => t.EventId == event1Id).ToList()
                },
                new EventDto()
                {
                    Id = event2Id,
                    Date = DateTime.Now,
                    EventSummaryId = event2SummaryId,
                    EventTypeId = event2TypeId,
                    LayoutId = null,
                    StatusId = Enums.StatusEnum.Active,
                    VenueId = venue2Id,
                    Tickets = Tickets.Where(t => t.EventId == event2Id).ToList()
                },
                new EventDto()
                {
                    Id = event3Id,
                    Date = DateTime.Now,
                    EventSummaryId = event3SummaryId,
                    EventTypeId = event3TypeId,
                    LayoutId = null,
                    StatusId = Enums.StatusEnum.Active,
                    VenueId = venue3Id,
                    Tickets = Tickets.Where(t => t.EventId == event3Id).ToList()
                }
            };


        }
    }
}
