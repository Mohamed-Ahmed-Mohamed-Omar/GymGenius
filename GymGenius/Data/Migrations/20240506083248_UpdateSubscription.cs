using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymGenius.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_plans_level_Trains_LevelId",
                table: "plans");

            migrationBuilder.DropForeignKey(
                name: "FK_plans_target_muscles_Target_MuscleId",
                table: "plans");

            migrationBuilder.DropForeignKey(
                name: "FK_plans_time_Ex_TimeId",
                table: "plans");

            migrationBuilder.DropForeignKey(
                name: "FK_shape_Trainings_target_muscles_Target_MuscleId",
                table: "shape_Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_subscriptions_target_muscles_Target_MuscleId",
                table: "subscriptions");

            migrationBuilder.DropTable(
                name: "target_muscles");

            migrationBuilder.DropIndex(
                name: "IX_subscriptions_Target_MuscleId",
                table: "subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_shape_Trainings_Target_MuscleId",
                table: "shape_Trainings");

            migrationBuilder.DropIndex(
                name: "IX_plans_Ex_TimeId",
                table: "plans");

            migrationBuilder.DropIndex(
                name: "IX_plans_LevelId",
                table: "plans");

            migrationBuilder.DropIndex(
                name: "IX_plans_Target_MuscleId",
                table: "plans");

            migrationBuilder.DropColumn(
                name: "Target_MuscleId",
                table: "shape_Trainings");

            migrationBuilder.DropColumn(
                name: "Ex_TimeId",
                table: "plans");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "plans");

            migrationBuilder.RenameColumn(
                name: "Target_MuscleId",
                table: "subscriptions",
                newName: "Subscription_Duration");

            migrationBuilder.RenameColumn(
                name: "Target_MuscleId",
                table: "plans",
                newName: "Num_of_Quotas");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "plans",
                newName: "Price");

            migrationBuilder.AddColumn<string>(
                name: "Name_Coach",
                table: "subscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Rest",
                table: "subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Subscription_Status",
                table: "subscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Time_End",
                table: "subscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Time_Start",
                table: "subscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name_Coach",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "Rest",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "Subscription_Status",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "Time_End",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "Time_Start",
                table: "subscriptions");

            migrationBuilder.RenameColumn(
                name: "Subscription_Duration",
                table: "subscriptions",
                newName: "Target_MuscleId");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "plans",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Num_of_Quotas",
                table: "plans",
                newName: "Target_MuscleId");

            migrationBuilder.AddColumn<int>(
                name: "Target_MuscleId",
                table: "shape_Trainings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ex_TimeId",
                table: "plans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "plans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "target_muscles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_target_muscles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_subscriptions_Target_MuscleId",
                table: "subscriptions",
                column: "Target_MuscleId");

            migrationBuilder.CreateIndex(
                name: "IX_shape_Trainings_Target_MuscleId",
                table: "shape_Trainings",
                column: "Target_MuscleId");

            migrationBuilder.CreateIndex(
                name: "IX_plans_Ex_TimeId",
                table: "plans",
                column: "Ex_TimeId");

            migrationBuilder.CreateIndex(
                name: "IX_plans_LevelId",
                table: "plans",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_plans_Target_MuscleId",
                table: "plans",
                column: "Target_MuscleId");

            migrationBuilder.AddForeignKey(
                name: "FK_plans_level_Trains_LevelId",
                table: "plans",
                column: "LevelId",
                principalTable: "level_Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_plans_target_muscles_Target_MuscleId",
                table: "plans",
                column: "Target_MuscleId",
                principalTable: "target_muscles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_plans_time_Ex_TimeId",
                table: "plans",
                column: "Ex_TimeId",
                principalTable: "time",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_shape_Trainings_target_muscles_Target_MuscleId",
                table: "shape_Trainings",
                column: "Target_MuscleId",
                principalTable: "target_muscles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_subscriptions_target_muscles_Target_MuscleId",
                table: "subscriptions",
                column: "Target_MuscleId",
                principalTable: "target_muscles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
