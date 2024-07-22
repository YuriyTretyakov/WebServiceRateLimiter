using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiExploration.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forecasts_ReportedEvents_EventId",
                table: "Forecasts");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedEvents_Reporters_ReporterId",
                table: "ReportedEvents");

            migrationBuilder.DropIndex(
                name: "IX_Forecasts_EventId",
                table: "Forecasts");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReporterId",
                table: "ReportedEvents",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ForecastId",
                table: "ReportedEvents",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventId",
                table: "Forecasts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Forecasts_EventId",
                table: "Forecasts",
                column: "EventId",
                unique: true,
                filter: "[EventId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Forecasts_ReportedEvents_EventId",
                table: "Forecasts",
                column: "EventId",
                principalTable: "ReportedEvents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedEvents_Reporters_ReporterId",
                table: "ReportedEvents",
                column: "ReporterId",
                principalTable: "Reporters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forecasts_ReportedEvents_EventId",
                table: "Forecasts");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedEvents_Reporters_ReporterId",
                table: "ReportedEvents");

            migrationBuilder.DropIndex(
                name: "IX_Forecasts_EventId",
                table: "Forecasts");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReporterId",
                table: "ReportedEvents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ForecastId",
                table: "ReportedEvents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EventId",
                table: "Forecasts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forecasts_EventId",
                table: "Forecasts",
                column: "EventId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Forecasts_ReportedEvents_EventId",
                table: "Forecasts",
                column: "EventId",
                principalTable: "ReportedEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedEvents_Reporters_ReporterId",
                table: "ReportedEvents",
                column: "ReporterId",
                principalTable: "Reporters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
