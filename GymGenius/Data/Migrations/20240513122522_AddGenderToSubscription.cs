using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymGenius.Migrations
{
    /// <inheritdoc />
    public partial class AddGenderToSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "GenderId",
                table: "subscriptions",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_subscriptions_GenderId",
                table: "subscriptions",
                column: "GenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptions_genders_GenderId",
                table: "subscriptions",
                column: "GenderId",
                principalTable: "genders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subscriptions_genders_GenderId",
                table: "subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_subscriptions_GenderId",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "subscriptions");
        }
    }
}
