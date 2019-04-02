using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Sql.Migrations
{
    public partial class FixingMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcedureResultColumnBindings_ObjectProperties_ObjectPropertyId",
                schema: "dbo",
                table: "ProcedureResultColumnBindings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcedureResultColumnBindings",
                schema: "dbo",
                table: "ProcedureResultColumnBindings");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                schema: "dbo",
                table: "ProcedureResultColumnBindings");

            migrationBuilder.AlterColumn<long>(
                name: "ObjectPropertyId",
                schema: "dbo",
                table: "ProcedureResultColumnBindings",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "ProcedureBindings",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcedureResultColumnBindings",
                schema: "dbo",
                table: "ProcedureResultColumnBindings",
                columns: new[] { "ResultColumnColumnId", "ObjectPropertyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProcedureResultColumnBindings_ObjectProperties_ObjectPropertyId",
                schema: "dbo",
                table: "ProcedureResultColumnBindings",
                column: "ObjectPropertyId",
                principalSchema: "dbo",
                principalTable: "ObjectProperties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcedureResultColumnBindings_ObjectProperties_ObjectPropertyId",
                schema: "dbo",
                table: "ProcedureResultColumnBindings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcedureResultColumnBindings",
                schema: "dbo",
                table: "ProcedureResultColumnBindings");

            migrationBuilder.AlterColumn<long>(
                name: "ObjectPropertyId",
                schema: "dbo",
                table: "ProcedureResultColumnBindings",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "PropertyId",
                schema: "dbo",
                table: "ProcedureResultColumnBindings",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "ProcedureBindings",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcedureResultColumnBindings",
                schema: "dbo",
                table: "ProcedureResultColumnBindings",
                columns: new[] { "ResultColumnColumnId", "PropertyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProcedureResultColumnBindings_ObjectProperties_ObjectPropertyId",
                schema: "dbo",
                table: "ProcedureResultColumnBindings",
                column: "ObjectPropertyId",
                principalSchema: "dbo",
                principalTable: "ObjectProperties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
