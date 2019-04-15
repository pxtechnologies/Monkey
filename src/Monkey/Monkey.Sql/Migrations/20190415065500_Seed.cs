using Microsoft.EntityFrameworkCore.Migrations;
using Monkey.Sql.Data;

namespace Monkey.Sql.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InitObjectTypes();
            migrationBuilder.InitSqlObjectMappings();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteSqlObjectMappings();
            migrationBuilder.DeleteObjectTypes();
        }
    }
}
