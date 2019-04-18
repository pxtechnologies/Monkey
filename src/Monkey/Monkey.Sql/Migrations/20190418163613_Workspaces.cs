using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Sql.Migrations
{
    public partial class Workspaces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compilations_Workspace_WorkspaceId",
                schema: "dbo",
                table: "Compilations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workspace",
                schema: "dbo",
                table: "Workspace");

            migrationBuilder.RenameTable(
                name: "Workspace",
                schema: "dbo",
                newName: "Workspaces",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Workspace_Status",
                schema: "dbo",
                table: "Workspaces",
                newName: "IX_Workspaces_Status");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workspaces",
                schema: "dbo",
                table: "Workspaces",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Compilations_Workspaces_WorkspaceId",
                schema: "dbo",
                table: "Compilations",
                column: "WorkspaceId",
                principalSchema: "dbo",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compilations_Workspaces_WorkspaceId",
                schema: "dbo",
                table: "Compilations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workspaces",
                schema: "dbo",
                table: "Workspaces");

            migrationBuilder.RenameTable(
                name: "Workspaces",
                schema: "dbo",
                newName: "Workspace",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Workspaces_Status",
                schema: "dbo",
                table: "Workspace",
                newName: "IX_Workspace_Status");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workspace",
                schema: "dbo",
                table: "Workspace",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Compilations_Workspace_WorkspaceId",
                schema: "dbo",
                table: "Compilations",
                column: "WorkspaceId",
                principalSchema: "dbo",
                principalTable: "Workspace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
