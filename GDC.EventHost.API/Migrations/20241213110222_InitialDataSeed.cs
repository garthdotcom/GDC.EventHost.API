using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GDC.EventHost.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Description", "EndDate", "LongDescription", "SeriesId", "StartDate", "StatusId", "Title" },
                values: new object[,]
                {
                    { new Guid("87a4b2be-eeab-4c59-a01d-e5fa1621706b"), "Prow scuttle parrel provost Sail ho shrouds spirits boom mizzenmast yardarm.", new DateTime(2025, 1, 2, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4360), "Prow scuttle parrel provost Sail ho shrouds spirits boom mizzenmast yardarm. Pinnace holystone mizzenmast quarter crow's nest nipperkin grog yardarm hempen halter furl. Swab barque interloper chantey doubloon starboard grog black jack gangway rutters.", new Guid("50108347-73eb-41a2-a9d0-3e0b78d2e432"), new DateTime(2024, 12, 18, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4149), 1, "Prow Scuttle Parrel" },
                    { new Guid("a0292853-7d7c-431f-9545-addb443296fd"), "Deadlights jack lad schooner scallywag dance the hempen jig carouser.", new DateTime(2025, 1, 2, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4586), "Deadlights jack lad schooner scallywag dance the hempen jig carouser broadside cable strike colors. Bring a spring upon her cable holystone blow the man down spanker Shiver me timbers to go on account lookout wherry doubloon chase. Belay yo-ho-ho keelhaul squiffy black spot yardarm spyglass sheet transom heave to.", new Guid("9bbe4e74-6ae9-489d-a345-d81f7e3708e2"), new DateTime(2024, 12, 18, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4584), 1, "Deadlights Jack Lad Schooner" },
                    { new Guid("bc6adac2-82ed-4a7e-9793-c60084439a65"), "Trysail Sail ho Corsair red ensign hulk smartly boom jib rum gangway.", new DateTime(2025, 1, 2, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4590), "Trysail Sail ho Corsair red ensign hulk smartly boom jib rum gangway. Case shot Shiver me timbers gangplank crack Jennys tea cup ballast Blimey lee snow crow's nest rutters. Fluke jib scourge of the seven seas boatswain schooner gaff booty Jack Tar transom spirits.", new Guid("460cec3a-6406-4f74-b9b2-065d3e140026"), new DateTime(2024, 12, 18, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4589), 1, "Trysail Sail ho Corsair" }
                });

            migrationBuilder.InsertData(
                table: "Performances",
                columns: new[] { "Id", "Date", "EventId", "PerformanceTypeId", "SeatingPlanId", "StatusId", "Title", "VenueId" },
                values: new object[,]
                {
                    { new Guid("3c2c43ea-09e9-4ff3-97e5-a1794cb8fa4d"), new DateTime(2024, 12, 20, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(8537), new Guid("87a4b2be-eeab-4c59-a01d-e5fa1621706b"), new Guid("af3bba31-bb59-4638-ad4b-dc62d7315d06"), null, 1, "Performance One", new Guid("76824d3e-78b7-4284-8117-a238c38c3dc7") },
                    { new Guid("778bd3f1-877a-497c-bb73-f4660a45482d"), new DateTime(2025, 1, 3, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(9135), new Guid("87a4b2be-eeab-4c59-a01d-e5fa1621706b"), new Guid("af3bba31-bb59-4638-ad4b-dc62d7315d06"), null, 1, "Performance Three", new Guid("76824d3e-78b7-4284-8117-a238c38c3dc7") },
                    { new Guid("bbb7c814-bd91-49e7-a3ba-5a22c2c90b43"), new DateTime(2024, 12, 23, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(9139), new Guid("a0292853-7d7c-431f-9545-addb443296fd"), new Guid("af3bba31-bb59-4638-ad4b-dc62d7315d06"), null, 1, "Performance Five", new Guid("76824d3e-78b7-4284-8117-a238c38c3dc7") },
                    { new Guid("e86e8347-d90b-4aec-a383-61f86a6675af"), new DateTime(2024, 12, 18, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(9137), new Guid("a0292853-7d7c-431f-9545-addb443296fd"), new Guid("af3bba31-bb59-4638-ad4b-dc62d7315d06"), null, 1, "Performance Four", new Guid("aa9d1367-1d71-4cb9-98b1-633b940967ee") },
                    { new Guid("f1463c64-37ba-4784-b40f-0efe753b5c0f"), new DateTime(2024, 12, 27, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(9131), new Guid("87a4b2be-eeab-4c59-a01d-e5fa1621706b"), new Guid("af3bba31-bb59-4638-ad4b-dc62d7315d06"), null, 1, "Performance Two", new Guid("76824d3e-78b7-4284-8117-a238c38c3dc7") },
                    { new Guid("f77a6b68-e2bb-4deb-abe1-b2d057b300e2"), new DateTime(2024, 12, 18, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(9142), new Guid("bc6adac2-82ed-4a7e-9793-c60084439a65"), new Guid("af3bba31-bb59-4638-ad4b-dc62d7315d06"), null, 1, "Performance Six", new Guid("aa9d1367-1d71-4cb9-98b1-633b940967ee") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("3c2c43ea-09e9-4ff3-97e5-a1794cb8fa4d"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("778bd3f1-877a-497c-bb73-f4660a45482d"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("bbb7c814-bd91-49e7-a3ba-5a22c2c90b43"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("e86e8347-d90b-4aec-a383-61f86a6675af"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("f1463c64-37ba-4784-b40f-0efe753b5c0f"));

            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("f77a6b68-e2bb-4deb-abe1-b2d057b300e2"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("87a4b2be-eeab-4c59-a01d-e5fa1621706b"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("a0292853-7d7c-431f-9545-addb443296fd"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("bc6adac2-82ed-4a7e-9793-c60084439a65"));
        }
    }
}
