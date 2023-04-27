using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegionWebApp.Migrations
{
    public partial class Secondary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HideMediaOverlay",
                table: "GalleryItems",
                newName: "MaxDisplay");

            migrationBuilder.AddColumn<int>(
                name: "ColWidth",
                table: "Media",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColWidth",
                table: "Media");

            migrationBuilder.RenameColumn(
                name: "MaxDisplay",
                table: "GalleryItems",
                newName: "HideMediaOverlay");
        }
    }
}
