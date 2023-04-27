using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegionWebApp.Migrations
{
    public partial class MediaCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Col",
                table: "Media",
                type: "text",
                nullable: false,
                defaultValue: "col");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColWidth",
                table: "Media",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
