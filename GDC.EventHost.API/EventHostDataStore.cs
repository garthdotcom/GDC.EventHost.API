using GDC.EventHost.DTO.Event;
using GDC.EventHost.DTO.Performance;

namespace GDC.EventHost.API
{
    public class EventHostDataStore
    {
        public List<EventDto> Events { get; set; }
        public List<PerformanceDto> Performances { get; set; }

        //public static EventHostDataStore Current { get; } = new EventHostDataStore();

        public EventHostDataStore()
        {
            var eventId1 = new Guid("87a4b2be-eeab-4c59-a01d-e5fa1621706b");
            var eventId2 = new Guid("a0292853-7d7c-431f-9545-addb443296fd");
            var eventId3 = new Guid("bc6adac2-82ed-4a7e-9793-c60084439a65");

            var performanceTypeId = new Guid("af3bba31-bb59-4638-ad4b-dc62d7315d06");

            var performanceId1 = new Guid("3c2c43ea-09e9-4ff3-97e5-a1794cb8fa4d");
            var performanceId2 = new Guid("f1463c64-37ba-4784-b40f-0efe753b5c0f");
            var performanceId3 = new Guid("778bd3f1-877a-497c-bb73-f4660a45482d");
            var performanceId4 = new Guid("e86e8347-d90b-4aec-a383-61f86a6675af");
            var performanceId5 = new Guid("bbb7c814-bd91-49e7-a3ba-5a22c2c90b43");
            var performanceId6 = new Guid("f77a6b68-e2bb-4deb-abe1-b2d057b300e2");

            var seriesId1 = new Guid("50108347-73eb-41a2-a9d0-3e0b78d2e432");
            var seriesId2 = new Guid("9bbe4e74-6ae9-489d-a345-d81f7e3708e2");
            var seriesId3 = new Guid("460cec3a-6406-4f74-b9b2-065d3e140026");

            var venueId1 = new Guid("76824d3e-78b7-4284-8117-a238c38c3dc7");
            var venueId2 = new Guid("aa9d1367-1d71-4cb9-98b1-633b940967ee");

            // init dummy data

            Performances = [
                new ()
                {
                    Id = performanceId1,
                    Date = DateTime.Now.AddDays(7),
                    EventId = eventId1,
                    PerformanceTypeId = performanceTypeId,
                    VenueId = venueId1,
                    Title = "Performance One"
                },
                new ()
                {
                    Id = performanceId2,
                    Date = DateTime.Now.AddDays(14),
                    EventId = eventId1,
                    PerformanceTypeId = performanceTypeId,
                    VenueId = venueId1,
                    Title = "Performance Two"
                },
                new ()
                {
                    Id = performanceId3,
                    Date = DateTime.Now.AddDays(21),
                    EventId = eventId1,
                    PerformanceTypeId = performanceTypeId,
                    VenueId = venueId1,
                    Title = "Performance Three"
                },
                new ()
                {
                    Id = performanceId4,
                    Date = DateTime.Now.AddDays(5),
                    EventId = eventId2,
                    PerformanceTypeId = performanceTypeId,
                    VenueId = venueId2,
                    Title = "Performance Four"
                },
                new ()
                {
                    Id = performanceId5,
                    Date = DateTime.Now.AddDays(10),
                    EventId = eventId2,
                    PerformanceTypeId = performanceTypeId,
                    VenueId = venueId1,
                    Title = "Performance Five"
                },
                new ()
                {
                    Id = performanceId6,
                    Date = DateTime.Now.AddDays(5),
                    EventId = eventId3,
                    PerformanceTypeId = performanceTypeId,
                    VenueId = venueId2,
                    Title = "Performance Six"
                }
            ];

            Events = [      
                new ()
                {
                    Id = eventId1,
                    Description = "This is Event One",
                    LongDescription = "This is Event One long",
                    SeriesId = seriesId1,
                    Title = "Event One",
                    StartDate = DateTime.Now.AddDays(5),
                    EndDate = DateTime.Now.AddDays(20),
                    Performances = Performances.Where(p => p.EventId == eventId1).ToList()
                },
                new ()
                {
                    Id = eventId2,
                    Description = "This is Event Two",
                    LongDescription = "This is Event Two long",
                    SeriesId = seriesId2,
                    Title = "Event Two",
                    StartDate = DateTime.Now.AddDays(5),
                    EndDate = DateTime.Now.AddDays(20),
                    Performances = Performances.Where(p => p.EventId == eventId2).ToList()
                },
                new ()
                {
                    Id = eventId3,
                    Description = "This is Event Three",
                    LongDescription = "This is Event Three long",
                    SeriesId = seriesId3,
                    Title = "Event Three",
                    StartDate = DateTime.Now.AddDays(5),
                    EndDate = DateTime.Now.AddDays(20),
                    Performances = Performances.Where(p => p.EventId == eventId3).ToList()
                }
            ];
        }
    }
}
