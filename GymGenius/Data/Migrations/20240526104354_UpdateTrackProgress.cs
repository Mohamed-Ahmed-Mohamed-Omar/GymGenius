using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymGenius.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrackProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
 

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "track_Progresses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
 

            migrationBuilder.DropColumn(
                name: "Time",
                table: "track_Progresses");

        }
    }
}
