using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymGenius.Migrations
{
    /// <inheritdoc />
    public partial class UpdatSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subscriptions_time_TimeId",
                table: "subscriptions");

            migrationBuilder.DropTable(
                name: "subscriptionsDays");

            migrationBuilder.DropTable(
                name: "time");

            migrationBuilder.DropTable(
                name: "days");

            migrationBuilder.DropIndex(
                name: "IX_subscriptions_TimeId",
                table: "subscriptions");


            migrationBuilder.DropColumn(
                name: "TimeId",
                table: "subscriptions");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "TimeId",
                table: "subscriptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "days",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_days", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "time",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_time", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "subscriptionsDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscriptionsDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_subscriptionsDays_days_DayId",
                        column: x => x.DayId,
                        principalTable: "days",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_subscriptionsDays_subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_subscriptions_TimeId",
                table: "subscriptions",
                column: "TimeId");

            migrationBuilder.CreateIndex(
                name: "IX_subscriptionsDays_DayId",
                table: "subscriptionsDays",
                column: "DayId");

            migrationBuilder.CreateIndex(
                name: "IX_subscriptionsDays_SubscriptionId",
                table: "subscriptionsDays",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptions_time_TimeId",
                table: "subscriptions",
                column: "TimeId",
                principalTable: "time",
                principalColumn: "Id");
        }
    }
}
