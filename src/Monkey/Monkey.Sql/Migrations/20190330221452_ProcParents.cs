using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Sql.Migrations
{
    public partial class ProcParents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProcedureId",
                table: "ProcedureResultDescriptors",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProcedureId",
                table: "ProcedureParameterDescriptors",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureResultDescriptors_ProcedureId",
                table: "ProcedureResultDescriptors",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureParameterDescriptors_ProcedureId",
                table: "ProcedureParameterDescriptors",
                column: "ProcedureId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcedureParameterDescriptors_ProcedureDescriptors_ProcedureId",
                table: "ProcedureParameterDescriptors",
                column: "ProcedureId",
                principalTable: "ProcedureDescriptors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcedureResultDescriptors_ProcedureDescriptors_ProcedureId",
                table: "ProcedureResultDescriptors",
                column: "ProcedureId",
                principalTable: "ProcedureDescriptors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcedureParameterDescriptors_ProcedureDescriptors_ProcedureId",
                table: "ProcedureParameterDescriptors");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcedureResultDescriptors_ProcedureDescriptors_ProcedureId",
                table: "ProcedureResultDescriptors");

            migrationBuilder.DropIndex(
                name: "IX_ProcedureResultDescriptors_ProcedureId",
                table: "ProcedureResultDescriptors");

            migrationBuilder.DropIndex(
                name: "IX_ProcedureParameterDescriptors_ProcedureId",
                table: "ProcedureParameterDescriptors");

            migrationBuilder.DropColumn(
                name: "ProcedureId",
                table: "ProcedureResultDescriptors");

            migrationBuilder.DropColumn(
                name: "ProcedureId",
                table: "ProcedureParameterDescriptors");
        }
    }
}
