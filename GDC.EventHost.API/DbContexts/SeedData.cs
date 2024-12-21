using GDC.EventHost.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GDC.EventHost.API.DbContexts
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new EventHostContext(
                serviceProvider.GetRequiredService<DbContextOptions<EventHostContext>>()))
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                var eventId1 = new Guid("87a4b2be-eeab-4c59-a01d-e5fa1621706b");
                var eventId2 = new Guid("a0292853-7d7c-431f-9545-addb443296fd");
                var eventId3 = new Guid("bc6adac2-82ed-4a7e-9793-c60084439a65");

                var performanceTypeId1 = new Guid("af3bba31-bb59-4638-ad4b-dc62d7315d06");
                var performanceTypeId2 = new Guid("872e6458-cf85-4370-bd00-c2ecd8eba886");
                var performanceTypeId3 = new Guid("680ceddb-3da1-474b-ba56-88b67d04668f");

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


                if (!context.PerformanceTypes.Any())
                {
                    context.PerformanceTypes.AddRange(
                        new PerformanceType()
                        {
                            Id = performanceTypeId1,
                            Name = "Concert",
                            Description = "Some text describing what the type Concert is about."
                        },
                        new PerformanceType()
                        {
                            Id = performanceTypeId2,
                            Name = "Film",
                            Description = "Some text describing what the type Film is about."
                        },
                        new PerformanceType()
                        {
                            Id = performanceTypeId3,
                            Name = "Lecture",
                            Description = "Some text describing what the type Lecture is about."
                        }
                    );
                }

                if (!context.Series.Any())
                {
                    context.Series.AddRange(
                        new Series()
                        {
                            Id = seriesId1,
                            Title = "Series One",
                            Description = "Integer tempus himenaeos suscipit penatibus mauris a a ultrices netus.",
                            LongDescription = "Integer tempus himenaeos suscipit penatibus mauris a a ultrices netus. Vel torquent iaculis dictum; hac sem habitant dis dictumst. Ligula mattis vulputate taciti vitae tellus. Dignissim felis cursus hac arcu ultricies. Primis vel quam interdum ut parturient eu proin. Eu justo rhoncus etiam pellentesque pretium varius. Aenean eros id senectus ligula ac praesent.",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(60),
                            StatusId = DTO.Enums.StatusEnum.Pending
                        },
                        new Series()
                        {
                            Id = seriesId2,
                            Title = "Series Two",
                            Description = "Sagittis varius justo aliquam dignissim nascetur mauris neque.",
                            LongDescription = "Sagittis varius justo aliquam dignissim nascetur mauris neque. Eleifend adipiscing vehicula fusce ac tempus himenaeos. Afusce habitasse sit magnis facilisi aptent lacinia. Magnis tristique lorem tincidunt cubilia aliquet. Ante mus consectetur vel id et. Dui euismod aenean porta varius diam ridiculus iaculis inceptos. Ut nostra augue mus imperdiet finibus gravida ex ipsum. Elementum metus lectus adipiscing; pretium mi ultricies magna. Conubia ad scelerisque venenatis a diam rutrum, sed congue egestas.",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(60),
                            StatusId = DTO.Enums.StatusEnum.Pending
                        },
                        new Series()
                        {
                            Id = seriesId3,
                            Title = "Series Three",
                            Description = "Facilisis nulla turpis proin fames fusce condimentum praesent lacus dui.",
                            LongDescription = "Facilisis nulla turpis proin fames fusce condimentum praesent lacus dui. Aliquam conubia porta sem semper elementum venenatis nam nisi risus. Mus sem fames dolor suspendisse interdum tincidunt adipiscing. Morbi in cursus diam rhoncus cursus nec ullamcorper eros. Dolor torquent elementum nullam mi varius magnis ultricies. Mauris etiam vehicula primis fringilla tortor tincidunt maximus integer rutrum. Taciti tellus efficitur dolor torquent ipsum sodales. Eros justo dui arcu potenti; senectus duis nunc.",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(60),
                            StatusId = DTO.Enums.StatusEnum.Pending
                        }
                    );
                    context.SaveChanges();
                }

                if (!context.Events.Any())
                {
                    context.Events.AddRange(
                        new Event()
                        {
                            Id = eventId1,
                            Title = "One : Prow Scuttle Parrel",
                            Description = "Prow scuttle parrel provost Sail ho shrouds spirits boom mizzenmast yardarm.",
                            LongDescription = "Prow scuttle parrel provost Sail ho shrouds spirits boom mizzenmast yardarm. Pinnace holystone mizzenmast quarter crow's nest nipperkin grog yardarm hempen halter furl. Swab barque interloper chantey doubloon starboard grog black jack gangway rutters.",
                            SeriesId = seriesId1,
                            StartDate = DateTime.Now.AddDays(5),
                            EndDate = DateTime.Now.AddDays(20),
                            StatusId = DTO.Enums.StatusEnum.Pending
                        },
                        new Event()
                        {
                            Id = eventId2,
                            Title = "Two : Deadlights Jack Lad Schooner",
                            Description = "Deadlights jack lad schooner scallywag dance the hempen jig carouser.",
                            LongDescription = "Deadlights jack lad schooner scallywag dance the hempen jig carouser broadside cable strike colors. Bring a spring upon her cable holystone blow the man down spanker Shiver me timbers to go on account lookout wherry doubloon chase. Belay yo-ho-ho keelhaul squiffy black spot yardarm spyglass sheet transom heave to.",
                            SeriesId = seriesId2,
                            StartDate = DateTime.Now.AddDays(5),
                            EndDate = DateTime.Now.AddDays(20),
                            StatusId = DTO.Enums.StatusEnum.Pending
                        },
                        new Event()
                        {
                            Id = eventId3,
                            Title = "Three : Trysail Sail ho Corsair",
                            Description = "Trysail Sail ho Corsair red ensign hulk smartly boom jib rum gangway.",
                            LongDescription = "Trysail Sail ho Corsair red ensign hulk smartly boom jib rum gangway. Case shot Shiver me timbers gangplank crack Jennys tea cup ballast Blimey lee snow crow's nest rutters. Fluke jib scourge of the seven seas boatswain schooner gaff booty Jack Tar transom spirits.",
                            SeriesId = seriesId3,
                            StartDate = DateTime.Now.AddDays(5),
                            EndDate = DateTime.Now.AddDays(20),
                            StatusId = DTO.Enums.StatusEnum.Pending
                        }
                    );
                    context.SaveChanges();
                }

                if (!context.Performances.Any())
                {
                    context.Performances.AddRange(
                        new Performance()
                        {
                            Id = performanceId1,
                            Title = "Event One - Performance One",
                            Date = DateTime.Now.AddDays(7),
                            Event = context.Events.First(e => e.Id == eventId1),
                            EventId = eventId1,
                            PerformanceType = context.PerformanceTypes.First(p => p.Id == performanceTypeId1),
                            PerformanceTypeId = performanceTypeId1,
                            VenueId = venueId1,
                            StatusId = DTO.Enums.StatusEnum.Pending,
                            SeatingPlanId = null
                        },
                        new Performance()
                        {
                            Id = performanceId2,
                            Title = "Event One - Performance Two",
                            Date = DateTime.Now.AddDays(14),
                            Event = context.Events.First(e => e.Id == eventId1),
                            EventId = eventId1,
                            PerformanceType = context.PerformanceTypes.First(p => p.Id == performanceTypeId1),
                            PerformanceTypeId = performanceTypeId1,
                            VenueId = venueId1,
                            StatusId = DTO.Enums.StatusEnum.Pending,
                            SeatingPlanId = null
                        },
                        new Performance()
                        {
                            Id = performanceId3,
                            Title = "Event One - Performance Three",
                            Date = DateTime.Now.AddDays(21),
                            Event = context.Events.First(e => e.Id == eventId1),
                            EventId = eventId1,
                            PerformanceType = context.PerformanceTypes.First(p => p.Id == performanceTypeId1),
                            PerformanceTypeId = performanceTypeId1,
                            VenueId = venueId1,
                            StatusId = DTO.Enums.StatusEnum.Pending,
                            SeatingPlanId = null
                        },
                        new Performance()
                        {
                            Id = performanceId4,
                            Title = "Event Two - Performance One",
                            Date = DateTime.Now.AddDays(28),
                            Event = context.Events.First(e => e.Id == eventId2),
                            EventId = eventId2,
                            PerformanceType = context.PerformanceTypes.First(p => p.Id == performanceTypeId2),
                            PerformanceTypeId = performanceTypeId2,
                            VenueId = venueId2,
                            StatusId = DTO.Enums.StatusEnum.Pending,
                            SeatingPlanId = null
                        },
                        new Performance()
                        {
                            Id = performanceId5,
                            Title = "Event Two - Performance Two",
                            Date = DateTime.Now.AddDays(35),
                            Event = context.Events.First(e => e.Id == eventId2),
                            EventId = eventId2,
                            PerformanceType = context.PerformanceTypes.First(p => p.Id == performanceTypeId2),
                            PerformanceTypeId = performanceTypeId2,
                            VenueId = venueId1,
                            StatusId = DTO.Enums.StatusEnum.Pending,
                            SeatingPlanId = null
                        },
                        new Performance()
                        {
                            Id = performanceId6,
                            Title = "Event Three - Performance One",
                            Date = DateTime.Now.AddDays(42),
                            Event = context.Events.First(e => e.Id == eventId3),
                            EventId = eventId3,
                            PerformanceType = context.PerformanceTypes.First(p => p.Id == performanceTypeId3),
                            PerformanceTypeId = performanceTypeId3,
                            VenueId = venueId2,
                            StatusId = DTO.Enums.StatusEnum.Pending,
                            SeatingPlanId = null
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
