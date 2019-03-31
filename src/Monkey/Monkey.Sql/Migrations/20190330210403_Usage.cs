using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Sql.Migrations
{
    public partial class Usage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "ObjectTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Usage",
                table: "ObjectTypes",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Usage",
                table: "ObjectTypes",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 16);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "ObjectTypes",
                maxLength: 16,
                nullable: false,
                defaultValue: "");
        }
    }
}
