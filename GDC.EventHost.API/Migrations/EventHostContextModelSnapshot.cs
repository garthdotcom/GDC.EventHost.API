﻿// <auto-generated />
using System;
using GDC.EventHost.API.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GDC.EventHost.API.Migrations
{
    [DbContext(typeof(EventHostContext))]
    partial class EventHostContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GDC.EventHost.API.Entities.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LongDescription")
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)");

                    b.Property<Guid?>("SeriesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.ToTable("Events");

                    b.HasData(
                        new
                        {
                            Id = new Guid("87a4b2be-eeab-4c59-a01d-e5fa1621706b"),
                            Description = "Prow scuttle parrel provost Sail ho shrouds spirits boom mizzenmast yardarm.",
                            EndDate = new DateTime(2025, 1, 2, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4360),
                            LongDescription = "Prow scuttle parrel provost Sail ho shrouds spirits boom mizzenmast yardarm. Pinnace holystone mizzenmast quarter crow's nest nipperkin grog yardarm hempen halter furl. Swab barque interloper chantey doubloon starboard grog black jack gangway rutters.",
                            SeriesId = new Guid("50108347-73eb-41a2-a9d0-3e0b78d2e432"),
                            StartDate = new DateTime(2024, 12, 18, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4149),
                            StatusId = 1,
                            Title = "Prow Scuttle Parrel"
                        },
                        new
                        {
                            Id = new Guid("a0292853-7d7c-431f-9545-addb443296fd"),
                            Description = "Deadlights jack lad schooner scallywag dance the hempen jig carouser.",
                            EndDate = new DateTime(2025, 1, 2, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4586),
                            LongDescription = "Deadlights jack lad schooner scallywag dance the hempen jig carouser broadside cable strike colors. Bring a spring upon her cable holystone blow the man down spanker Shiver me timbers to go on account lookout wherry doubloon chase. Belay yo-ho-ho keelhaul squiffy black spot yardarm spyglass sheet transom heave to.",
                            SeriesId = new Guid("9bbe4e74-6ae9-489d-a345-d81f7e3708e2"),
                            StartDate = new DateTime(2024, 12, 18, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4584),
                            StatusId = 1,
                            Title = "Deadlights Jack Lad Schooner"
                        },
                        new
                        {
                            Id = new Guid("bc6adac2-82ed-4a7e-9793-c60084439a65"),
                            Description = "Trysail Sail ho Corsair red ensign hulk smartly boom jib rum gangway.",
                            EndDate = new DateTime(2025, 1, 2, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4590),
                            LongDescription = "Trysail Sail ho Corsair red ensign hulk smartly boom jib rum gangway. Case shot Shiver me timbers gangplank crack Jennys tea cup ballast Blimey lee snow crow's nest rutters. Fluke jib scourge of the seven seas boatswain schooner gaff booty Jack Tar transom spirits.",
                            SeriesId = new Guid("460cec3a-6406-4f74-b9b2-065d3e140026"),
                            StartDate = new DateTime(2024, 12, 18, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4589),
                            StatusId = 1,
                            Title = "Trysail Sail ho Corsair"
                        });
                });

            modelBuilder.Entity("GDC.EventHost.API.Entities.Performance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PerformanceTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SeatingPlanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<Guid?>("VenueId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("Performances");

                    b.HasData(
                        new
                        {
                            Id = new Guid("3c2c43ea-09e9-4ff3-97e5-a1794cb8fa4d"),
                            Date = new DateTime(2024, 12, 20, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(8537),
                            EventId = new Guid("87a4b2be-eeab-4c59-a01d-e5fa1621706b"),
                            PerformanceTypeId = new Guid("af3bba31-bb59-4638-ad4b-dc62d7315d06"),
                            StatusId = 1,
                            Title = "Performance One",
                            VenueId = new Guid("76824d3e-78b7-4284-8117-a238c38c3dc7")
                        },
                        new
                        {
                            Id = new Guid("f1463c64-37ba-4784-b40f-0efe753b5c0f"),
                            Date = new DateTime(2024, 12, 27, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(9131),
                            EventId = new Guid("87a4b2be-eeab-4c59-a01d-e5fa1621706b"),
                            PerformanceTypeId = new Guid("af3bba31-bb59-4638-ad4b-dc62d7315d06"),
                            StatusId = 1,
                            Title = "Performance Two",
                            VenueId = new Guid("76824d3e-78b7-4284-8117-a238c38c3dc7")
                        },
                        new
                        {
                            Id = new Guid("778bd3f1-877a-497c-bb73-f4660a45482d"),
                            Date = new DateTime(2025, 1, 3, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(9135),
                            EventId = new Guid("87a4b2be-eeab-4c59-a01d-e5fa1621706b"),
                            PerformanceTypeId = new Guid("af3bba31-bb59-4638-ad4b-dc62d7315d06"),
                            StatusId = 1,
                            Title = "Performance Three",
                            VenueId = new Guid("76824d3e-78b7-4284-8117-a238c38c3dc7")
                        },
                        new
                        {
                            Id = new Guid("e86e8347-d90b-4aec-a383-61f86a6675af"),
                            Date = new DateTime(2024, 12, 18, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(9137),
                            EventId = new Guid("a0292853-7d7c-431f-9545-addb443296fd"),
                            PerformanceTypeId = new Guid("af3bba31-bb59-4638-ad4b-dc62d7315d06"),
                            StatusId = 1,
                            Title = "Performance Four",
                            VenueId = new Guid("aa9d1367-1d71-4cb9-98b1-633b940967ee")
                        },
                        new
                        {
                            Id = new Guid("bbb7c814-bd91-49e7-a3ba-5a22c2c90b43"),
                            Date = new DateTime(2024, 12, 23, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(9139),
                            EventId = new Guid("a0292853-7d7c-431f-9545-addb443296fd"),
                            PerformanceTypeId = new Guid("af3bba31-bb59-4638-ad4b-dc62d7315d06"),
                            StatusId = 1,
                            Title = "Performance Five",
                            VenueId = new Guid("76824d3e-78b7-4284-8117-a238c38c3dc7")
                        },
                        new
                        {
                            Id = new Guid("f77a6b68-e2bb-4deb-abe1-b2d057b300e2"),
                            Date = new DateTime(2024, 12, 18, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(9142),
                            EventId = new Guid("bc6adac2-82ed-4a7e-9793-c60084439a65"),
                            PerformanceTypeId = new Guid("af3bba31-bb59-4638-ad4b-dc62d7315d06"),
                            StatusId = 1,
                            Title = "Performance Six",
                            VenueId = new Guid("aa9d1367-1d71-4cb9-98b1-633b940967ee")
                        });
                });

            modelBuilder.Entity("GDC.EventHost.API.Entities.Performance", b =>
                {
                    b.HasOne("GDC.EventHost.API.Entities.Event", "Event")
                        .WithMany("Performances")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("GDC.EventHost.API.Entities.Event", b =>
                {
                    b.Navigation("Performances");
                });
#pragma warning restore 612, 618
        }
    }
}
