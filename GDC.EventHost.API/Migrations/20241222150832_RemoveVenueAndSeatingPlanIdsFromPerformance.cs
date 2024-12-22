using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GDC.EventHost.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveVenueAndSeatingPlanIdsFromPerformance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatingPlanId",
                table: "Performances");

            migrationBuilder.DropColumn(
                name: "VenueId",
                table: "Performances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SeatingPlanId",
                table: "Performances",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VenueId",
                table: "Performances",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
