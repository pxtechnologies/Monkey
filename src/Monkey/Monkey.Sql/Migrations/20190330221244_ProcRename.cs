using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Sql.Migrations
{
    public partial class ProcRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcedureBindings_Procedures_ProcedureId",
                table: "ProcedureBindings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Procedures",
                table: "Procedures");

            migrationBuilder.RenameTable(
                name: "Procedures",
                newName: "ProcedureDescriptors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcedureDescriptors",
                table: "ProcedureDescriptors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcedureBindings_ProcedureDescriptors_ProcedureId",
                table: "ProcedureBindings",
                column: "ProcedureId",
                principalTable: "ProcedureDescriptors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcedureBindings_ProcedureDescriptors_ProcedureId",
                table: "ProcedureBindings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcedureDescriptors",
                table: "ProcedureDescriptors");

            migrationBuilder.RenameTable(
                name: "ProcedureDescriptors",
                newName: "Procedures");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Procedures",
                table: "Procedures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcedureBindings_Procedures_ProcedureId",
                table: "ProcedureBindings",
                column: "ProcedureId",
                principalTable: "Procedures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
