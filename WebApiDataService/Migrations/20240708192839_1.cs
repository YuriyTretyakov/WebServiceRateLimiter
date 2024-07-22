using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiExploration.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportedEvents_Forecasts_ForecastId",
                table: "ReportedEvents");

            migrationBuilder.DropIndex(
                name: "IX_ReportedEvents_ForecastId",
                table: "ReportedEvents");

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Forecasts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ForecastReporterId",
                table: "Forecasts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forecasts_EventId",
                table: "Forecasts",
                column: "EventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forecasts_ForecastReporterId",
                table: "Forecasts",
                column: "ForecastReporterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Forecasts_ReportedEvents_EventId",
                table: "Forecasts",
                column: "EventId",
                principalTable: "ReportedEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Forecasts_Reporters_ForecastReporterId",
                table: "Forecasts",
                column: "ForecastReporterId",
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
                name: "FK_Forecasts_Reporters_ForecastReporterId",
                table: "Forecasts");

            migrationBuilder.DropIndex(
                name: "IX_Forecasts_EventId",
                table: "Forecasts");

            migrationBuilder.DropIndex(
                name: "IX_Forecasts_ForecastReporterId",
                table: "Forecasts");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Forecasts");

            migrationBuilder.DropColumn(
                name: "ForecastReporterId",
                table: "Forecasts");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedEvents_ForecastId",
                table: "ReportedEvents",
                column: "ForecastId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedEvents_Forecasts_ForecastId",
                table: "ReportedEvents",
                column: "ForecastId",
                principalTable: "Forecasts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
