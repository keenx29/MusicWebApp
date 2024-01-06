using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicWebApp.Migrations
{
    public partial class RenameLengthColumnToDuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Length",
                table: "Songs",
                newName: "Duration");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Songs",
                newName: "Length");
        }
    }
}
