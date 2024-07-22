using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiExploration.Migrations
{
    /// <inheritdoc />
    public partial class AddReporterandReporterEventTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reporters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reporters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ReporterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ForecastId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportedEvents_Forecasts_ForecastId",
                        column: x => x.ForecastId,
                        principalTable: "Forecasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedEvents_Reporters_ReporterId",
                        column: x => x.ReporterId,
                        principalTable: "Reporters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportedEvents_ForecastId",
                table: "ReportedEvents",
                column: "ForecastId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedEvents_Id",
                table: "ReportedEvents",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportedEvents_ReporterId",
                table: "ReportedEvents",
                column: "ReporterId");

            migrationBuilder.CreateIndex(
                name: "IX_Reporters_Id",
                table: "Reporters",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reporters_Name",
                table: "Reporters",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportedEvents");

            migrationBuilder.DropTable(
                name: "Reporters");
        }
    }
}
