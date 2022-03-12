using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStoreWebApp.Migrations
{
    public partial class ChangeInColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookID",
                table: "Gallery");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookID",
                table: "Gallery",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
