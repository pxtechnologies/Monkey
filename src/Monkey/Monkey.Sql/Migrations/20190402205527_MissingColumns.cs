using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Sql.Migrations
{
    public partial class MissingColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcedureBindings_ObjectTypes_ResultId",
                schema: "dbo",
                table: "ProcedureBindings");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "ProcedureBindings",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RequestId",
                schema: "dbo",
                table: "ProcedureBindings",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureBindings_RequestId",
                schema: "dbo",
                table: "ProcedureBindings",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcedureBindings_ObjectTypes_RequestId",
                schema: "dbo",
                table: "ProcedureBindings",
                column: "RequestId",
                principalSchema: "dbo",
                principalTable: "ObjectTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcedureBindings_ObjectTypes_ResultId",
                schema: "dbo",
                table: "ProcedureBindings",
                column: "ResultId",
                principalSchema: "dbo",
                principalTable: "ObjectTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcedureBindings_ObjectTypes_RequestId",
                schema: "dbo",
                table: "ProcedureBindings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcedureBindings_ObjectTypes_ResultId",
                schema: "dbo",
                table: "ProcedureBindings");

            migrationBuilder.DropIndex(
                name: "IX_ProcedureBindings_RequestId",
                schema: "dbo",
                table: "ProcedureBindings");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "dbo",
                table: "ProcedureBindings");

            migrationBuilder.DropColumn(
                name: "RequestId",
                schema: "dbo",
                table: "ProcedureBindings");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcedureBindings_ObjectTypes_ResultId",
                schema: "dbo",
                table: "ProcedureBindings",
                column: "ResultId",
                principalSchema: "dbo",
                principalTable: "ObjectTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
