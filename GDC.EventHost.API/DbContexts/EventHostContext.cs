using GDC.EventHost.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GDC.EventHost.API.DbContexts
{
    public class EventHostContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        public DbSet<Performance> Performances { get; set; }

        public EventHostContext(DbContextOptions<EventHostContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
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

            modelBuilder.Entity<Event>()
                .HasData(
                    new Event("Prow Scuttle Parrel")
                    {
                        Id = eventId1,
                        Description = "Prow scuttle parrel provost Sail ho shrouds spirits boom mizzenmast yardarm.",
                        LongDescription = "Prow scuttle parrel provost Sail ho shrouds spirits boom mizzenmast yardarm. Pinnace holystone mizzenmast quarter crow's nest nipperkin grog yardarm hempen halter furl. Swab barque interloper chantey doubloon starboard grog black jack gangway rutters.",
                        SeriesId = seriesId1,
                        StartDate = DateTime.Now.AddDays(5),
                        EndDate = DateTime.Now.AddDays(20),
                        StatusId = DTO.Enums.StatusEnum.Pending
                    },
                    new Event("Deadlights Jack Lad Schooner")
                    {
                        Id = eventId2,
                        Description = "Deadlights jack lad schooner scallywag dance the hempen jig carouser.",
                        LongDescription = "Deadlights jack lad schooner scallywag dance the hempen jig carouser broadside cable strike colors. Bring a spring upon her cable holystone blow the man down spanker Shiver me timbers to go on account lookout wherry doubloon chase. Belay yo-ho-ho keelhaul squiffy black spot yardarm spyglass sheet transom heave to.",
                        SeriesId = seriesId2,
                        StartDate = DateTime.Now.AddDays(5),
                        EndDate = DateTime.Now.AddDays(20),
                        StatusId = DTO.Enums.StatusEnum.Pending
                    },
                    new Event("Trysail Sail ho Corsair")
                    {
                        Id = eventId3,
                        Description = "Trysail Sail ho Corsair red ensign hulk smartly boom jib rum gangway.",
                        LongDescription = "Trysail Sail ho Corsair red ensign hulk smartly boom jib rum gangway. Case shot Shiver me timbers gangplank crack Jennys tea cup ballast Blimey lee snow crow's nest rutters. Fluke jib scourge of the seven seas boatswain schooner gaff booty Jack Tar transom spirits.",
                        SeriesId = seriesId3,
                        StartDate = DateTime.Now.AddDays(5),
                        EndDate = DateTime.Now.AddDays(20),
                        StatusId = DTO.Enums.StatusEnum.Pending
                    }
                );

            modelBuilder.Entity<Performance>()
                .HasData(
                    new Performance("Performance One")
                    {
                        Id = performanceId1,
                        Date = DateTime.Now.AddDays(7),
                        EventId = eventId1,
                        PerformanceTypeId = performanceTypeId,
                        VenueId = venueId1,
                        StatusId = DTO.Enums.StatusEnum.Pending,
                        SeatingPlanId = null
                    },
                    new Performance("Performance Two")
                    {
                        Id = performanceId2,
                        Date = DateTime.Now.AddDays(14),
                        EventId = eventId1,
                        PerformanceTypeId = performanceTypeId,
                        VenueId = venueId1,
                        StatusId = DTO.Enums.StatusEnum.Pending,
                        SeatingPlanId = null
                    },
                    new Performance("Performance Three")
                    {
                        Id = performanceId3,
                        Date = DateTime.Now.AddDays(21),
                        EventId = eventId1,
                        PerformanceTypeId = performanceTypeId,
                        VenueId = venueId1,
                        StatusId = DTO.Enums.StatusEnum.Pending,
                        SeatingPlanId = null
                    },
                    new Performance("Performance Four")
                    {
                        Id = performanceId4,
                        Date = DateTime.Now.AddDays(5),
                        EventId = eventId2,
                        PerformanceTypeId = performanceTypeId,
                        VenueId = venueId2,
                        StatusId = DTO.Enums.StatusEnum.Pending,
                        SeatingPlanId = null
                    },
                    new Performance("Performance Five")
                    {
                        Id = performanceId5,
                        Date = DateTime.Now.AddDays(10),
                        EventId = eventId2,
                        PerformanceTypeId = performanceTypeId,
                        VenueId = venueId1,
                        StatusId = DTO.Enums.StatusEnum.Pending,
                        SeatingPlanId = null
                    },
                    new Performance("Performance Six")
                    {
                        Id = performanceId6,
                        Date = DateTime.Now.AddDays(5),
                        EventId = eventId3,
                        PerformanceTypeId = performanceTypeId,
                        VenueId = venueId2,
                        StatusId = DTO.Enums.StatusEnum.Pending,
                        SeatingPlanId = null
                    }
                );

            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("connectionstring");

        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
