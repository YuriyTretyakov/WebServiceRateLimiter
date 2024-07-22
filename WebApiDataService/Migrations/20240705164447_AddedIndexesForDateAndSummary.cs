using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiExploration.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexesForDateAndSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "Forecasts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forecasts_Date_Summary",
                table: "Forecasts",
                columns: new[] { "Date", "Summary" });

            migrationBuilder.CreateIndex(
                name: "IX_Forecasts_Id",
                table: "Forecasts",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Forecasts_Date_Summary",
                table: "Forecasts");

            migrationBuilder.DropIndex(
                name: "IX_Forecasts_Id",
                table: "Forecasts");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "Forecasts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
