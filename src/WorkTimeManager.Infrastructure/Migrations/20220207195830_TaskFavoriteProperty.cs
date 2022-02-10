using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkTimeManager.Infrastructure.Migrations
{
    public partial class TaskFavoriteProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavorites",
                table: "Tasks",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavorites",
                table: "Tasks");
        }
    }
}
