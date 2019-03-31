using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Sql.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "ProcedureResultDescriptors",
                newName: "ProcedureResultDescriptors",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ProcedureResultColumnBindings",
                newName: "ProcedureResultColumnBindings",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ProcedureParameterDescriptors",
                newName: "ProcedureParameterDescriptors",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ProcedureParameterBindings",
                newName: "ProcedureParameterBindings",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ProcedureDescriptors",
                newName: "ProcedureDescriptors",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ProcedureBindings",
                newName: "ProcedureBindings",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ObjectTypes",
                newName: "ObjectTypes",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ObjectProperties",
                newName: "ObjectProperties",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Controllers",
                newName: "Controllers",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ControllerActions",
                newName: "ControllerActions",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ActionParameterBindings",
                newName: "ActionParameterBindings",
                newSchema: "dbo");

            migrationBuilder.InsertData("ObjectTypes",
                new string[] {"Usage", "IsPrimitive", "Name", "IsVoid", "IsDynamic"},
                new object[,]
                {
                    {"Primitive", 1, "string", 0 , 0},
                    {"Primitive", 1, "int" , 0, 0},
                    {"Primitive", 1, "double", 0, 0 },
                    {"Primitive", 1, "float", 0 , 0},
                    {"Primitive", 1, "DateTime" , 0, 0},
                    {"Primitive", 1, "TimeSpan", 0 , 0},
                    {"Primitive", 1, "DateTimeOffset", 0, 0 },
                    {"Primitive", 1, "Decimal" , 0, 0}
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ProcedureResultDescriptors",
                schema: "dbo",
                newName: "ProcedureResultDescriptors");

            migrationBuilder.RenameTable(
                name: "ProcedureResultColumnBindings",
                schema: "dbo",
                newName: "ProcedureResultColumnBindings");

            migrationBuilder.RenameTable(
                name: "ProcedureParameterDescriptors",
                schema: "dbo",
                newName: "ProcedureParameterDescriptors");

            migrationBuilder.RenameTable(
                name: "ProcedureParameterBindings",
                schema: "dbo",
                newName: "ProcedureParameterBindings");

            migrationBuilder.RenameTable(
                name: "ProcedureDescriptors",
                schema: "dbo",
                newName: "ProcedureDescriptors");

            migrationBuilder.RenameTable(
                name: "ProcedureBindings",
                schema: "dbo",
                newName: "ProcedureBindings");

            migrationBuilder.RenameTable(
                name: "ObjectTypes",
                schema: "dbo",
                newName: "ObjectTypes");

            migrationBuilder.RenameTable(
                name: "ObjectProperties",
                schema: "dbo",
                newName: "ObjectProperties");

            migrationBuilder.RenameTable(
                name: "Controllers",
                schema: "dbo",
                newName: "Controllers");

            migrationBuilder.RenameTable(
                name: "ControllerActions",
                schema: "dbo",
                newName: "ControllerActions");

            migrationBuilder.RenameTable(
                name: "ActionParameterBindings",
                schema: "dbo",
                newName: "ActionParameterBindings");
        }
    }
}
