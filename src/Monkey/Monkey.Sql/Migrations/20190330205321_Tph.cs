using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Sql.Migrations
{
    public partial class Tph : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionParameterBindings_ObjectType_ControllerRequestId",
                table: "ActionParameterBindings");

            migrationBuilder.DropForeignKey(
                name: "FK_ControllerActions_ObjectType_ControllerResponseId",
                table: "ControllerActions");

            migrationBuilder.DropForeignKey(
                name: "FK_ObjectProperties_ObjectType_DeclaringTypeId",
                table: "ObjectProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_ObjectProperties_ObjectType_PropertyTypeId",
                table: "ObjectProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcedureBindings_ObjectType_ResultId",
                table: "ProcedureBindings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ObjectType",
                table: "ObjectType");

            migrationBuilder.RenameTable(
                name: "ObjectType",
                newName: "ObjectTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "ObjectTypes",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ObjectTypes",
                table: "ObjectTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionParameterBindings_ObjectTypes_ControllerRequestId",
                table: "ActionParameterBindings",
                column: "ControllerRequestId",
                principalTable: "ObjectTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ControllerActions_ObjectTypes_ControllerResponseId",
                table: "ControllerActions",
                column: "ControllerResponseId",
                principalTable: "ObjectTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ObjectProperties_ObjectTypes_DeclaringTypeId",
                table: "ObjectProperties",
                column: "DeclaringTypeId",
                principalTable: "ObjectTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ObjectProperties_ObjectTypes_PropertyTypeId",
                table: "ObjectProperties",
                column: "PropertyTypeId",
                principalTable: "ObjectTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcedureBindings_ObjectTypes_ResultId",
                table: "ProcedureBindings",
                column: "ResultId",
                principalTable: "ObjectTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionParameterBindings_ObjectTypes_ControllerRequestId",
                table: "ActionParameterBindings");

            migrationBuilder.DropForeignKey(
                name: "FK_ControllerActions_ObjectTypes_ControllerResponseId",
                table: "ControllerActions");

            migrationBuilder.DropForeignKey(
                name: "FK_ObjectProperties_ObjectTypes_DeclaringTypeId",
                table: "ObjectProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_ObjectProperties_ObjectTypes_PropertyTypeId",
                table: "ObjectProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcedureBindings_ObjectTypes_ResultId",
                table: "ProcedureBindings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ObjectTypes",
                table: "ObjectTypes");

            migrationBuilder.RenameTable(
                name: "ObjectTypes",
                newName: "ObjectType");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "ObjectType",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 16);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ObjectType",
                table: "ObjectType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionParameterBindings_ObjectType_ControllerRequestId",
                table: "ActionParameterBindings",
                column: "ControllerRequestId",
                principalTable: "ObjectType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ControllerActions_ObjectType_ControllerResponseId",
                table: "ControllerActions",
                column: "ControllerResponseId",
                principalTable: "ObjectType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ObjectProperties_ObjectType_DeclaringTypeId",
                table: "ObjectProperties",
                column: "DeclaringTypeId",
                principalTable: "ObjectType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ObjectProperties_ObjectType_PropertyTypeId",
                table: "ObjectProperties",
                column: "PropertyTypeId",
                principalTable: "ObjectType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcedureBindings_ObjectType_ResultId",
                table: "ProcedureBindings",
                column: "ResultId",
                principalTable: "ObjectType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
