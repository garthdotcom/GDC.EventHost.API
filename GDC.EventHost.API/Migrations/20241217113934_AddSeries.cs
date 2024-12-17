using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GDC.EventHost.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSeries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    LongDescription = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("87a4b2be-eeab-4c59-a01d-e5fa1621706b"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 1, 6, 11, 39, 34, 380, DateTimeKind.Local).AddTicks(1016), new DateTime(2024, 12, 22, 11, 39, 34, 380, DateTimeKind.Local).AddTicks(909) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("a0292853-7d7c-431f-9545-addb443296fd"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 1, 6, 11, 39, 34, 380, DateTimeKind.Local).AddTicks(1213), new DateTime(2024, 12, 22, 11, 39, 34, 380, DateTimeKind.Local).AddTicks(1211) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("bc6adac2-82ed-4a7e-9793-c60084439a65"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 1, 6, 11, 39, 34, 380, DateTimeKind.Local).AddTicks(1216), new DateTime(2024, 12, 22, 11, 39, 34, 380, DateTimeKind.Local).AddTicks(1215) });

            migrationBuilder.UpdateData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("3c2c43ea-09e9-4ff3-97e5-a1794cb8fa4d"),
                column: "Date",
                value: new DateTime(2024, 12, 24, 11, 39, 34, 380, DateTimeKind.Local).AddTicks(1664));

            migrationBuilder.UpdateData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("778bd3f1-877a-497c-bb73-f4660a45482d"),
                column: "Date",
                value: new DateTime(2025, 1, 7, 11, 39, 34, 380, DateTimeKind.Local).AddTicks(2339));

            migrationBuilder.UpdateData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("bbb7c814-bd91-49e7-a3ba-5a22c2c90b43"),
                column: "Date",
                value: new DateTime(2024, 12, 27, 11, 39, 34, 380, DateTimeKind.Local).AddTicks(2343));

            migrationBuilder.UpdateData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("e86e8347-d90b-4aec-a383-61f86a6675af"),
                column: "Date",
                value: new DateTime(2024, 12, 22, 11, 39, 34, 380, DateTimeKind.Local).AddTicks(2341));

            migrationBuilder.UpdateData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("f1463c64-37ba-4784-b40f-0efe753b5c0f"),
                column: "Date",
                value: new DateTime(2024, 12, 31, 11, 39, 34, 380, DateTimeKind.Local).AddTicks(2333));

            migrationBuilder.UpdateData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("f77a6b68-e2bb-4deb-abe1-b2d057b300e2"),
                column: "Date",
                value: new DateTime(2024, 12, 22, 11, 39, 34, 380, DateTimeKind.Local).AddTicks(2345));

            migrationBuilder.InsertData(
                table: "Series",
                columns: new[] { "Id", "Description", "EndDate", "LongDescription", "StartDate", "StatusId", "Title" },
                values: new object[,]
                {
                    { new Guid("460cec3a-6406-4f74-b9b2-065d3e140026"), "Facilisis nulla turpis proin fames fusce condimentum praesent lacus dui.", new DateTime(2025, 2, 15, 11, 39, 34, 379, DateTimeKind.Local).AddTicks(6449), "Facilisis nulla turpis proin fames fusce condimentum praesent lacus dui. Aliquam conubia porta sem semper elementum venenatis nam nisi risus. Mus sem fames dolor suspendisse interdum tincidunt adipiscing. Morbi in cursus diam rhoncus cursus nec ullamcorper eros. Dolor torquent elementum nullam mi varius magnis ultricies. Mauris etiam vehicula primis fringilla tortor tincidunt maximus integer rutrum. Taciti tellus efficitur dolor torquent ipsum sodales. Eros justo dui arcu potenti; senectus duis nunc.", new DateTime(2024, 12, 17, 11, 39, 34, 379, DateTimeKind.Local).AddTicks(6448), 1, "Series Three" },
                    { new Guid("50108347-73eb-41a2-a9d0-3e0b78d2e432"), "Integer tempus himenaeos suscipit penatibus mauris a a ultrices netus.", new DateTime(2025, 2, 15, 11, 39, 34, 379, DateTimeKind.Local).AddTicks(6243), "Integer tempus himenaeos suscipit penatibus mauris a a ultrices netus. Vel torquent iaculis dictum; hac sem habitant dis dictumst. Ligula mattis vulputate taciti vitae tellus. Dignissim felis cursus hac arcu ultricies. Primis vel quam interdum ut parturient eu proin. Eu justo rhoncus etiam pellentesque pretium varius. Aenean eros id senectus ligula ac praesent.", new DateTime(2024, 12, 17, 11, 39, 34, 379, DateTimeKind.Local).AddTicks(5992), 1, "Series One" },
                    { new Guid("9bbe4e74-6ae9-489d-a345-d81f7e3708e2"), "Sagittis varius justo aliquam dignissim nascetur mauris neque.", new DateTime(2025, 2, 15, 11, 39, 34, 379, DateTimeKind.Local).AddTicks(6446), "Sagittis varius justo aliquam dignissim nascetur mauris neque. Eleifend adipiscing vehicula fusce ac tempus himenaeos. Afusce habitasse sit magnis facilisi aptent lacinia. Magnis tristique lorem tincidunt cubilia aliquet. Ante mus consectetur vel id et. Dui euismod aenean porta varius diam ridiculus iaculis inceptos. Ut nostra augue mus imperdiet finibus gravida ex ipsum. Elementum metus lectus adipiscing; pretium mi ultricies magna. Conubia ad scelerisque venenatis a diam rutrum, sed congue egestas.", new DateTime(2024, 12, 17, 11, 39, 34, 379, DateTimeKind.Local).AddTicks(6443), 1, "Series Two" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_SeriesId",
                table: "Events",
                column: "SeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Series_SeriesId",
                table: "Events",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Series_SeriesId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Events_SeriesId",
                table: "Events");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("87a4b2be-eeab-4c59-a01d-e5fa1621706b"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 1, 2, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4360), new DateTime(2024, 12, 18, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4149) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("a0292853-7d7c-431f-9545-addb443296fd"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 1, 2, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4586), new DateTime(2024, 12, 18, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4584) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("bc6adac2-82ed-4a7e-9793-c60084439a65"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 1, 2, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4590), new DateTime(2024, 12, 18, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(4589) });

            migrationBuilder.UpdateData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("3c2c43ea-09e9-4ff3-97e5-a1794cb8fa4d"),
                column: "Date",
                value: new DateTime(2024, 12, 20, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(8537));

            migrationBuilder.UpdateData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("778bd3f1-877a-497c-bb73-f4660a45482d"),
                column: "Date",
                value: new DateTime(2025, 1, 3, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(9135));

            migrationBuilder.UpdateData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("bbb7c814-bd91-49e7-a3ba-5a22c2c90b43"),
                column: "Date",
                value: new DateTime(2024, 12, 23, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(9139));

            migrationBuilder.UpdateData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("e86e8347-d90b-4aec-a383-61f86a6675af"),
                column: "Date",
                value: new DateTime(2024, 12, 18, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(9137));

            migrationBuilder.UpdateData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("f1463c64-37ba-4784-b40f-0efe753b5c0f"),
                column: "Date",
                value: new DateTime(2024, 12, 27, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(9131));

            migrationBuilder.UpdateData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("f77a6b68-e2bb-4deb-abe1-b2d057b300e2"),
                column: "Date",
                value: new DateTime(2024, 12, 18, 11, 2, 21, 680, DateTimeKind.Local).AddTicks(9142));
        }
    }
}
