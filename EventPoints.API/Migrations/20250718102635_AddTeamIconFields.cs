using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventPoints.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamIconFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageMimeType",
                table: "Teams",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TeamImage",
                table: "Teams",
                type: "BLOB",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageMimeType",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamImage",
                table: "Teams");
        }
    }
}
