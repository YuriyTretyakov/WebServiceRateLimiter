using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiExploration.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forecasts_Reporters_ForecastReporterId",
                table: "Forecasts");

            migrationBuilder.RenameColumn(
                name: "ForecastReporterId",
                table: "Forecasts",
                newName: "ReporterId");

            migrationBuilder.RenameIndex(
                name: "IX_Forecasts_ForecastReporterId",
                table: "Forecasts",
                newName: "IX_Forecasts_ReporterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Forecasts_Reporters_ReporterId",
                table: "Forecasts",
                column: "ReporterId",
                principalTable: "Reporters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forecasts_Reporters_ReporterId",
                table: "Forecasts");

            migrationBuilder.RenameColumn(
                name: "ReporterId",
                table: "Forecasts",
                newName: "ForecastReporterId");

            migrationBuilder.RenameIndex(
                name: "IX_Forecasts_ReporterId",
                table: "Forecasts",
                newName: "IX_Forecasts_ForecastReporterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Forecasts_Reporters_ForecastReporterId",
                table: "Forecasts",
                column: "ForecastReporterId",
                principalTable: "Reporters",
                principalColumn: "Id");
        }
    }
}
