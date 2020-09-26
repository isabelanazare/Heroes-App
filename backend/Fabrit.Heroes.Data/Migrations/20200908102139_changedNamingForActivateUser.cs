using Microsoft.EntityFrameworkCore.Migrations;

namespace Fabrit.Heroes.Data.Migrations
{
    public partial class changedNamingForActivateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activated",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "IsActivated",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActivated",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "Activated",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
