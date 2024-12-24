using GDC.EventHost.API.Entities;
using GDC.EventHost.DTO;
using Microsoft.EntityFrameworkCore;

namespace GDC.EventHost.API.DbContexts
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new EventHostContext(
                serviceProvider.GetRequiredService<DbContextOptions<EventHostContext>>()))
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                var performanceTypeId_concert = new Guid("af3bba31-bb59-4638-ad4b-dc62d7315d06");
                var performanceTypeId_film = new Guid("872e6458-cf85-4370-bd00-c2ecd8eba886");
                var performanceTypeId_lecture = new Guid("680ceddb-3da1-474b-ba56-88b67d04668f");

                var seriesId_masterworks = new Guid("50108347-73eb-41a2-a9d0-3e0b78d2e432");
                var seriesId_french_cinema = new Guid("9bbe4e74-6ae9-489d-a345-d81f7e3708e2");
                var seriesId_tarantino = new Guid("460cec3a-6406-4f74-b9b2-065d3e140026");

                var eventId_symphony_6 = new Guid("87a4b2be-eeab-4c59-a01d-e5fa1621706b");
                var eventId_parapluies = new Guid("a0292853-7d7c-431f-9545-addb443296fd");
                var eventId_souffle = new Guid("f90a14d2-75eb-44d1-b545-57185131b8c5");
                var eventId_pulp_fiction = new Guid("bc6adac2-82ed-4a7e-9793-c60084439a65");
                var eventId_reservoir_dogs = new Guid("cbd9640f-ee35-41e6-be8d-af3404963a0c");
                var eventId_kill_bill_1 = new Guid("53f67c6b-e0f3-4799-82a0-8ab68b6625f7");

                var venueId_neptune = new Guid("76824d3e-78b7-4284-8117-a238c38c3dc7");
                var venueId_egyptian = new Guid("aa9d1367-1d71-4cb9-98b1-633b940967ee");
                var venueId_benaroya = new Guid("3b8e8d21-c879-4824-a57a-a36fe7ffc468");

                var performanceId1 = new Guid("3c2c43ea-09e9-4ff3-97e5-a1794cb8fa4d");
                var performanceId2 = new Guid("f1463c64-37ba-4784-b40f-0efe753b5c0f");
                var performanceId3 = new Guid("778bd3f1-877a-497c-bb73-f4660a45482d");
                var performanceId4 = new Guid("e86e8347-d90b-4aec-a383-61f86a6675af");
                var performanceId5 = new Guid("bbb7c814-bd91-49e7-a3ba-5a22c2c90b43");
                var performanceId6 = new Guid("f77a6b68-e2bb-4deb-abe1-b2d057b300e2");
                var performanceId7 = new Guid("e07e913c-2256-443c-bcb1-3b7e9a53f89a");
                var performanceId8 = new Guid("f1bec050-bc40-4e22-bfea-38e560e6e7ea");
                var performanceId9 = new Guid("2413db0b-12ba-4b3f-abd8-6c0e9b182488");
                var performanceId10 = new Guid("235974b9-dd2f-42cf-8039-01eb68274c96");


                if (!context.Statuses.Any())
                {
                    context.Statuses.AddRange(
                        new Status()
                        {
                            Id = Enums.StatusEnum.Active,
                            Name = Enums.StatusEnum.Active.ToString()
                        },
                        new Status()
                        {
                            Id = Enums.StatusEnum.Pending,
                            Name = Enums.StatusEnum.Pending.ToString()
                        },
                        new Status()
                        {
                            Id = Enums.StatusEnum.Deleted,
                            Name = Enums.StatusEnum.Deleted.ToString()
                        }
                     );
                    await context.SaveChangesAsync();
                }

                if (!context.PerformanceTypes.Any())
                {
                    context.PerformanceTypes.AddRange(
                        new PerformanceType()
                        {
                            Id = performanceTypeId_concert,
                            Name = "Concert",
                            Description = "Some text describing what the type Concert is about."
                        },
                        new PerformanceType()
                        {
                            Id = performanceTypeId_film,
                            Name = "Film",
                            Description = "Some text describing what the type Film is about."
                        },
                        new PerformanceType()
                        {
                            Id = performanceTypeId_lecture,
                            Name = "Lecture",
                            Description = "Some text describing what the type Lecture is about."
                        }
                    );
                    await context.SaveChangesAsync();
                }

                if (!context.Series.Any())
                {
                    context.Series.AddRange(
                        new Series()
                        {
                            Id = seriesId_masterworks,
                            Title = "Beethoven's Masterworks",
                            Description = "Integer tempus himenaeos suscipit penatibus mauris a a ultrices netus.",
                            LongDescription = "Integer tempus himenaeos suscipit penatibus mauris a a ultrices netus. Vel torquent iaculis dictum; hac sem habitant dis dictumst. Ligula mattis vulputate taciti vitae tellus. Dignissim felis cursus hac arcu ultricies. Primis vel quam interdum ut parturient eu proin. Eu justo rhoncus etiam pellentesque pretium varius. Aenean eros id senectus ligula ac praesent.",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(60),
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active)
                        },
                        new Series()
                        {
                            Id = seriesId_french_cinema,
                            Title = "La Nouvelle Vague - 1960's French Cinema",
                            Description = "Sagittis varius justo aliquam dignissim nascetur mauris neque.",
                            LongDescription = "Sagittis varius justo aliquam dignissim nascetur mauris neque. Eleifend adipiscing vehicula fusce ac tempus himenaeos. Afusce habitasse sit magnis facilisi aptent lacinia. Magnis tristique lorem tincidunt cubilia aliquet. Ante mus consectetur vel id et. Dui euismod aenean porta varius diam ridiculus iaculis inceptos. Ut nostra augue mus imperdiet finibus gravida ex ipsum. Elementum metus lectus adipiscing; pretium mi ultricies magna. Conubia ad scelerisque venenatis a diam rutrum, sed congue egestas.",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(60),
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active)
                        },
                        new Series()
                        {
                            Id = seriesId_tarantino,
                            Title = "Director Spotlight - Quentin Tarantino",
                            Description = "Facilisis nulla turpis proin fames fusce condimentum praesent lacus dui.",
                            LongDescription = "Facilisis nulla turpis proin fames fusce condimentum praesent lacus dui. Aliquam conubia porta sem semper elementum venenatis nam nisi risus. Mus sem fames dolor suspendisse interdum tincidunt adipiscing. Morbi in cursus diam rhoncus cursus nec ullamcorper eros. Dolor torquent elementum nullam mi varius magnis ultricies. Mauris etiam vehicula primis fringilla tortor tincidunt maximus integer rutrum. Taciti tellus efficitur dolor torquent ipsum sodales. Eros justo dui arcu potenti; senectus duis nunc.",
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(60),
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active)
                        }
                    );
                    await context.SaveChangesAsync();
                }

                if (!context.Events.Any())
                {
                    context.Events.AddRange(
                        new Event()
                        {
                            Id = eventId_symphony_6,
                            Title = "Beethoven Symphony No 6 - Pastoral",
                            Description = "Prow scuttle parrel provost Sail ho shrouds spirits boom mizzenmast yardarm.",
                            LongDescription = "Prow scuttle parrel provost Sail ho shrouds spirits boom mizzenmast yardarm. Pinnace holystone mizzenmast quarter crow's nest nipperkin grog yardarm hempen halter furl. Swab barque interloper chantey doubloon starboard grog black jack gangway rutters.",
                            SeriesId = seriesId_masterworks,
                            StartDate = DateTime.Now.AddDays(5),
                            EndDate = DateTime.Now.AddDays(20),
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active)
                        },
                        new Event()
                        {
                            Id = eventId_parapluies,
                            Title = "Les Parapluies de Cherbourg",
                            Description = "Deadlights jack lad schooner scallywag dance the hempen jig carouser.",
                            LongDescription = "Deadlights jack lad schooner scallywag dance the hempen jig carouser broadside cable strike colors. Bring a spring upon her cable holystone blow the man down spanker Shiver me timbers to go on account lookout wherry doubloon chase. Belay yo-ho-ho keelhaul squiffy black spot yardarm spyglass sheet transom heave to.",
                            SeriesId = seriesId_french_cinema,
                            StartDate = DateTime.Now.AddDays(5),
                            EndDate = DateTime.Now.AddDays(20),
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active)
                        },
                        new Event()
                        {
                            Id = eventId_souffle,
                            Title = "À Bout de Souffle",
                            Description = "Deadlights jack lad schooner scallywag dance the hempen jig carouser.",
                            LongDescription = "Deadlights jack lad schooner scallywag dance the hempen jig carouser broadside cable strike colors. Bring a spring upon her cable holystone blow the man down spanker Shiver me timbers to go on account lookout wherry doubloon chase. Belay yo-ho-ho keelhaul squiffy black spot yardarm spyglass sheet transom heave to.",
                            SeriesId = seriesId_french_cinema,
                            StartDate = DateTime.Now.AddDays(5),
                            EndDate = DateTime.Now.AddDays(20),
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active)
                        },
                        new Event()
                        {
                            Id = eventId_pulp_fiction,
                            Title = "Pulp Fiction",
                            Description = "Trysail Sail ho Corsair red ensign hulk smartly boom jib rum gangway.",
                            LongDescription = "Trysail Sail ho Corsair red ensign hulk smartly boom jib rum gangway. Case shot Shiver me timbers gangplank crack Jennys tea cup ballast Blimey lee snow crow's nest rutters. Fluke jib scourge of the seven seas boatswain schooner gaff booty Jack Tar transom spirits.",
                            SeriesId = seriesId_tarantino,
                            StartDate = DateTime.Now.AddDays(5),
                            EndDate = DateTime.Now.AddDays(20),
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active)
                        },
                        new Event()
                        {
                            Id = eventId_reservoir_dogs,
                            Title = "Reservoir Dogs",
                            Description = "Trysail Sail ho Corsair red ensign hulk smartly boom jib rum gangway.",
                            LongDescription = "Trysail Sail ho Corsair red ensign hulk smartly boom jib rum gangway. Case shot Shiver me timbers gangplank crack Jennys tea cup ballast Blimey lee snow crow's nest rutters. Fluke jib scourge of the seven seas boatswain schooner gaff booty Jack Tar transom spirits.",
                            SeriesId = seriesId_tarantino,
                            StartDate = DateTime.Now.AddDays(5),
                            EndDate = DateTime.Now.AddDays(20),
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active)
                        },
                        new Event()
                        {
                            Id = eventId_kill_bill_1,
                            Title = "Kill Bill: Volume 1",
                            Description = "Trysail Sail ho Corsair red ensign hulk smartly boom jib rum gangway.",
                            LongDescription = "Trysail Sail ho Corsair red ensign hulk smartly boom jib rum gangway. Case shot Shiver me timbers gangplank crack Jennys tea cup ballast Blimey lee snow crow's nest rutters. Fluke jib scourge of the seven seas boatswain schooner gaff booty Jack Tar transom spirits.",
                            SeriesId = seriesId_tarantino,
                            StartDate = DateTime.Now.AddDays(5),
                            EndDate = DateTime.Now.AddDays(20),
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active)
                        }
                    );
                    await context.SaveChangesAsync();
                }

                if (!context.Venues.Any())
                {
                    context.Venues.AddRange(
                        new Venue()
                        {
                            Id = venueId_neptune,
                            Name = "Neptune Theater",
                            Description = "1303 NE 45th Street, Seattle WA",
                            LongDescription = "The Neptune Theatre, formerly known as U-Neptune Theatre, is a performing arts venue in the University District neighborhood of Seattle, Washington, United States. Opened in 1921, the 1,000 capacity venue hosts a variety of events, including dance and music performances, film screenings, and arts education. It was primarily used for screening classic films prior to a 2011 renovation. In 2014, the theater and building were designated a Seattle landmark.",
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active)
                        },
                        new Venue()
                        {
                            Id = venueId_egyptian,
                            Name = "Egyptian Theater",
                            Description = "805 E Pine Street, Seattle WA 98122",
                            LongDescription = "The Egyptian Theater, officially the SIFF Cinema Egyptian, is a movie theater in the Capitol Hill neighborhood of Seattle, Washington, United States. The theater is operated by the Seattle International Film Festival (SIFF) and located on Pine Street near the Seattle Central College campus. The theater is located in a historic Masonic Temple, which opened in 1916 and served several local lodges. The four-story brick and terra cotta building included a 1,800-seat auditorium designed by B. Marcus Priteca that was used for community events. The auditorium was renovated by SIFF and decorated in an Egyptian theme; it reopened on November 14, 1980, as the 520-seat Egyptian Theater, with a screening of the French film Charles and Lucie.",
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active)
                        },
                        new Venue()
                        {
                            Id = venueId_benaroya,
                            Name = "Benaroya Hall",
                            Description = "200 University Street, Seattle WA",
                            LongDescription = "Benaroya Hall is the home of the Seattle Symphony in Downtown Seattle, Washington, United States. It features two auditoria, the S. Mark Taper Foundation Auditorium, a 2,500-seat performance venue, as well as the Illsley Ball Nordstrom Recital Hall, which seats 536. Opened in September 1998 at a cost of $120 million, Benaroya is noted for its technology-infused acoustics designed by Cyril Harris. Benaroya occupies an entire city block in the center of the city and has helped double the Seattle Symphony's budget and number of performances. The lobby of the hall features a large contribution of glass art, such as one given the title Crystal Cascade, by artist Dale Chihuly.",
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active)
                        }
                    );
                    await context.SaveChangesAsync();
                }

                if (!context.Performances.Any())
                {
                    context.Performances.AddRange(
                        new Performance()
                        {
                            Id = performanceId1,
                            Date = DateTime.Now.AddDays(7),
                            Event = context.Events.First(e => e.Id == eventId_symphony_6),
                            EventId = eventId_symphony_6,
                            PerformanceType = context.PerformanceTypes.First(p => p.Id == performanceTypeId_concert),
                            PerformanceTypeId = performanceTypeId_concert,
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active),
                            VenueId = venueId_benaroya,
                            Venue = context.Venues.First(v => v.Id == venueId_benaroya)
                        },
                        new Performance()
                        {
                            Id = performanceId2,
                            Date = DateTime.Now.AddDays(2),
                            Event = context.Events.First(e => e.Id == eventId_parapluies),
                            EventId = eventId_parapluies,
                            PerformanceType = context.PerformanceTypes.First(p => p.Id == performanceTypeId_film),
                            PerformanceTypeId = performanceTypeId_film,
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active),
                            VenueId = venueId_neptune,
                            Venue = context.Venues.First(v => v.Id == venueId_neptune)
                        },
                        new Performance()
                        {
                            Id = performanceId3,
                            Date = DateTime.Now.AddDays(4),
                            Event = context.Events.First(e => e.Id == eventId_parapluies),
                            EventId = eventId_parapluies,
                            PerformanceType = context.PerformanceTypes.First(p => p.Id == performanceTypeId_film),
                            PerformanceTypeId = performanceTypeId_film,
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active),
                            VenueId = venueId_egyptian,
                            Venue = context.Venues.First(v => v.Id == venueId_egyptian)
                        },
                        new Performance()
                        {
                            Id = performanceId4,
                            Date = DateTime.Now.AddDays(3),
                            Event = context.Events.First(e => e.Id == eventId_souffle),
                            EventId = eventId_souffle,
                            PerformanceType = context.PerformanceTypes.First(p => p.Id == performanceTypeId_film),
                            PerformanceTypeId = performanceTypeId_film,
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active),
                            VenueId = venueId_neptune,
                            Venue = context.Venues.First(v => v.Id == venueId_neptune)
                        },
                        new Performance()
                        {
                            Id = performanceId5,
                            Date = DateTime.Now.AddDays(6),
                            Event = context.Events.First(e => e.Id == eventId_souffle),
                            EventId = eventId_souffle,
                            PerformanceType = context.PerformanceTypes.First(p => p.Id == performanceTypeId_film),
                            PerformanceTypeId = performanceTypeId_film,
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active),
                            VenueId = venueId_egyptian,
                            Venue = context.Venues.First(v => v.Id == venueId_egyptian)
                        },
                        new Performance()
                        {
                            Id = performanceId6,
                            Date = DateTime.Now.AddDays(4),
                            Event = context.Events.First(e => e.Id == eventId_pulp_fiction),
                            EventId = eventId_pulp_fiction,
                            PerformanceType = context.PerformanceTypes.First(p => p.Id == performanceTypeId_film),
                            PerformanceTypeId = performanceTypeId_film,
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active),
                            VenueId = venueId_neptune,
                            Venue = context.Venues.First(v => v.Id == venueId_neptune)
                        },
                        new Performance()
                        {
                            Id = performanceId7,
                            Date = DateTime.Now.AddDays(8),
                            Event = context.Events.First(e => e.Id == eventId_pulp_fiction),
                            EventId = eventId_pulp_fiction,
                            PerformanceType = context.PerformanceTypes.First(p => p.Id == performanceTypeId_film),
                            PerformanceTypeId = performanceTypeId_film,
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active),
                            VenueId = venueId_egyptian,
                            Venue = context.Venues.First(v => v.Id == venueId_neptune)
                        },
                        new Performance()
                        {
                            Id = performanceId8,
                            Date = DateTime.Now.AddDays(5),
                            Event = context.Events.First(e => e.Id == eventId_reservoir_dogs),
                            EventId = eventId_reservoir_dogs,
                            PerformanceType = context.PerformanceTypes.First(p => p.Id == performanceTypeId_film),
                            PerformanceTypeId = performanceTypeId_film,
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active),
                            VenueId = venueId_neptune,
                            Venue = context.Venues.First(v => v.Id == venueId_neptune)
                        },
                        new Performance()
                        {
                            Id = performanceId9,
                            Date = DateTime.Now.AddDays(10),
                            Event = context.Events.First(e => e.Id == eventId_kill_bill_1),
                            EventId = eventId_kill_bill_1,
                            PerformanceType = context.PerformanceTypes.First(p => p.Id == performanceTypeId_film),
                            PerformanceTypeId = performanceTypeId_film,
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active),
                            VenueId = venueId_egyptian,
                            Venue = context.Venues.First(v => v.Id == venueId_egyptian)
                        },
                        new Performance()
                        {
                            Id = performanceId10,
                            Date = DateTime.Now.AddDays(12),
                            Event = context.Events.First(e => e.Id == eventId_kill_bill_1),
                            EventId = eventId_kill_bill_1,
                            PerformanceType = context.PerformanceTypes.First(p => p.Id == performanceTypeId_film),
                            PerformanceTypeId = performanceTypeId_film,
                            StatusId = Enums.StatusEnum.Active,
                            Status = context.Statuses.First(s => s.Id == Enums.StatusEnum.Active),
                            VenueId = venueId_egyptian,
                            Venue = context.Venues.First(v => v.Id == venueId_egyptian)
                        }
                    );
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
