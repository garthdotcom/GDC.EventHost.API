using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GDC.EventHost.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStatusId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Performances");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Events");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Series",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Performances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
