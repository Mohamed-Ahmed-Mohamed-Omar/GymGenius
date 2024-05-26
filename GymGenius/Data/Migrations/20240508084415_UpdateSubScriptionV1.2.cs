using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymGenius.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubScriptionV12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subscriptions_time_TimeId",
                table: "subscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "TimeId",
                table: "subscriptions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "subscriptions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptions_time_TimeId",
                table: "subscriptions",
                column: "TimeId",
                principalTable: "time",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subscriptions_time_TimeId",
                table: "subscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "TimeId",
                table: "subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptions_time_TimeId",
                table: "subscriptions",
                column: "TimeId",
                principalTable: "time",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
