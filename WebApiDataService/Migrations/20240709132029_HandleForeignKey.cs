using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiExploration.Migrations
{
    /// <inheritdoc />
    public partial class HandleForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportedEvents_Forecasts_ForecastId",
                table: "ReportedEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedEvents_Reporters_ReporterId",
                table: "ReportedEvents");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedEvents_Forecasts_ForecastId",
                table: "ReportedEvents",
                column: "ForecastId",
                principalTable: "Forecasts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedEvents_Reporters_ReporterId",
                table: "ReportedEvents",
                column: "ReporterId",
                principalTable: "Reporters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportedEvents_Forecasts_ForecastId",
                table: "ReportedEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedEvents_Reporters_ReporterId",
                table: "ReportedEvents");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedEvents_Forecasts_ForecastId",
                table: "ReportedEvents",
                column: "ForecastId",
                principalTable: "Forecasts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedEvents_Reporters_ReporterId",
                table: "ReportedEvents",
                column: "ReporterId",
                principalTable: "Reporters",
                principalColumn: "Id");
        }
    }
}
