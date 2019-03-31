using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Sql.Migrations
{
    public partial class TphOptimization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Usage",
                table: "ObjectTypes",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Usage",
                table: "ObjectTypes");
        }
    }
}
