using GDC.EventHost.DTO;
using GDC.EventHost.DTO.Event;
using GDC.EventHost.DTO.Performance;

namespace GDC.EventHost.API
{
    public class EventHostDataStore
    {
        public List<EventDto> Events { get; set; }
        public List<PerformanceDto> Performances { get; set; }

        public static EventHostDataStore Current { get; } = new EventHostDataStore();

        public EventHostDataStore()
        {
            var event1Id = Guid.NewGuid();
            var event2Id = Guid.NewGuid();
            var event3Id = Guid.NewGuid();
            var performanceTypeId = Guid.NewGuid();

            var series1Id = Guid.NewGuid();
            var series2Id = Guid.NewGuid();
            var series3Id = Guid.NewGuid();

            var event1TypeId = Guid.NewGuid();
            var event2TypeId = Guid.NewGuid();
            var event3TypeId = Guid.NewGuid();
            var venue1Id = Guid.NewGuid();
            var venue2Id = Guid.NewGuid();
            var venue3Id = Guid.NewGuid();

            // init dummy data
            Events = new List<EventDto>()
            {
                new EventDto()
                {
                    Id = event1Id,
                    Description = "This is Event One",
                    LongDescription = "This is Event One long",
                    SeriesId = series1Id,
                    StatusId = Enums.StatusEnum.Pending,
                    Title = "Event One",
                    StartDate = DateTime.Now.AddDays(5),
                    EndDate = DateTime.Now.AddDays(20),
                    Performances = [
                        new ()
                        {
                            Id = Guid.NewGuid(),
                            Date = DateTime.Now.AddDays(7),
                            EventId = event1Id,
                            PerformanceTypeId = performanceTypeId,
                            StatusId = Enums.StatusEnum.Pending,
                            VenueId = venue1Id
                        },
                        new ()
                        {
                            Id = Guid.NewGuid(),
                            Date = DateTime.Now.AddDays(14),
                            EventId = event1Id,
                            PerformanceTypeId = performanceTypeId,
                            StatusId = Enums.StatusEnum.Pending,
                            VenueId = venue1Id
                        },
                        new ()
                        {
                            Id = Guid.NewGuid(),
                            Date = DateTime.Now.AddDays(21),
                            EventId = event1Id,
                            PerformanceTypeId = performanceTypeId,
                            StatusId = Enums.StatusEnum.Pending,
                            VenueId = venue1Id
                        }
                    ]
                },
                new EventDto()
                {
                    Id = event2Id,
                    Description = "This is Event Two",
                    LongDescription = "This is Event Two long",
                    SeriesId = series2Id,
                    StatusId = Enums.StatusEnum.Pending,
                    Title = "Event Two",
                    StartDate = DateTime.Now.AddDays(5),
                    EndDate = DateTime.Now.AddDays(20),
                    Performances = [
                        new ()
                        {
                            Id = Guid.NewGuid(),
                            Date = DateTime.Now.AddDays(5),
                            EventId = event2Id,
                            PerformanceTypeId = performanceTypeId,
                            StatusId = Enums.StatusEnum.Pending,
                            VenueId = venue2Id
                        },
                        new ()
                        {
                            Id = Guid.NewGuid(),
                            Date = DateTime.Now.AddDays(10),
                            EventId = event2Id,
                            PerformanceTypeId = performanceTypeId,
                            StatusId = Enums.StatusEnum.Pending,
                            VenueId = venue3Id
                        }
                    ]
                },
                new EventDto()
                {
                    Id = event3Id,
                    Description = "This is Event Three",
                    LongDescription = "This is Event Three long",
                    SeriesId = series3Id,
                    StatusId = Enums.StatusEnum.Pending,
                    Title = "Event Three",
                    StartDate = DateTime.Now.AddDays(5),
                    EndDate = DateTime.Now.AddDays(20),
                    Performances = [
                        new ()
                        {
                            Id = Guid.NewGuid(),
                            Date = DateTime.Now.AddDays(5),
                            EventId = event3Id,
                            PerformanceTypeId = performanceTypeId,
                            StatusId = Enums.StatusEnum.Pending,
                            VenueId = venue3Id
                        }
                    ]
                }
            };
        }
    }
}
