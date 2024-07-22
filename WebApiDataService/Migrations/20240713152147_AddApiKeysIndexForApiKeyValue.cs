using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiDataService.Migrations
{
    /// <inheritdoc />
    public partial class AddApiKeysIndexForApiKeyValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApiKeyValue",
                table: "ApiKeys",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_ApiKeyValue",
                table: "ApiKeys",
                column: "ApiKeyValue",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApiKeys_ApiKeyValue",
                table: "ApiKeys");

            migrationBuilder.AlterColumn<string>(
                name: "ApiKeyValue",
                table: "ApiKeys",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
