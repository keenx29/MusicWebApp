using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicWebApp.Migrations
{
    public partial class AddImageURLColumnToPlaylists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Playlists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Playlists");
        }
    }
}
