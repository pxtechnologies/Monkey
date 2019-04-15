using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Sql.Migrations
{
    public partial class InitSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence(
                name: "HiLo",
                schema: "dbo",
                startValue: 1000L,
                incrementBy: 100);

            migrationBuilder.CreateTable(
                name: "Compilations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    CompiledAt = table.Column<DateTimeOffset>(nullable: false),
                    CompilationDuration = table.Column<TimeSpan>(nullable: false),
                    Hash = table.Column<Guid>(nullable: false),
                    Source = table.Column<string>(nullable: true),
                    Purpose = table.Column<int>(nullable: false),
                    Assembly = table.Column<byte[]>(nullable: true),
                    Classes = table.Column<string>(nullable: true),
                    ServerName = table.Column<string>(maxLength: 255, nullable: true),
                    Errors = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compilations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Controllers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Route = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controllers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObjectTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Namespace = table.Column<string>(maxLength: 255, nullable: true),
                    Alias = table.Column<string>(maxLength: 32, nullable: true),
                    IsPrimitive = table.Column<bool>(nullable: false),
                    IsVoid = table.Column<bool>(nullable: false),
                    IsDynamic = table.Column<bool>(nullable: false),
                    Usage = table.Column<string>(maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureDescriptors",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    ConnectionName = table.Column<string>(maxLength: 255, nullable: false),
                    Schema = table.Column<string>(maxLength: 255, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureDescriptors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ControllerActions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Route = table.Column<string>(maxLength: 255, nullable: true),
                    Verb = table.Column<int>(nullable: false),
                    ResponseId = table.Column<long>(nullable: false),
                    ControllerResponseId = table.Column<long>(nullable: true),
                    IsResponseCollection = table.Column<bool>(nullable: false),
                    ControllerDescriptorId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControllerActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ControllerActions_Controllers_ControllerDescriptorId",
                        column: x => x.ControllerDescriptorId,
                        principalSchema: "dbo",
                        principalTable: "Controllers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ControllerActions_ObjectTypes_ControllerResponseId",
                        column: x => x.ControllerResponseId,
                        principalSchema: "dbo",
                        principalTable: "ObjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ObjectProperties",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    PropertyTypeId = table.Column<long>(nullable: false),
                    IsCollection = table.Column<bool>(nullable: false),
                    DeclaringTypeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectProperties_ObjectTypes_DeclaringTypeId",
                        column: x => x.DeclaringTypeId,
                        principalSchema: "dbo",
                        principalTable: "ObjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ObjectProperties_ObjectTypes_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalSchema: "dbo",
                        principalTable: "ObjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SqlObjectTypeMappings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    ObjectTypeId = table.Column<long>(nullable: false),
                    SqlType = table.Column<string>(maxLength: 255, nullable: true),
                    IsNullable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SqlObjectTypeMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SqlObjectTypeMappings_ObjectTypes_ObjectTypeId",
                        column: x => x.ObjectTypeId,
                        principalSchema: "dbo",
                        principalTable: "ObjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureBindings",
                schema: "dbo",
                columns: table => new
                {
                    ProcedureId = table.Column<long>(nullable: false),
                    ResultId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    RequestId = table.Column<long>(nullable: false),
                    IsResultCollection = table.Column<bool>(nullable: false),
                    Mode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureBindings", x => new { x.ProcedureId, x.ResultId });
                    table.ForeignKey(
                        name: "FK_ProcedureBindings_ProcedureDescriptors_ProcedureId",
                        column: x => x.ProcedureId,
                        principalSchema: "dbo",
                        principalTable: "ProcedureDescriptors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcedureBindings_ObjectTypes_RequestId",
                        column: x => x.RequestId,
                        principalSchema: "dbo",
                        principalTable: "ObjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcedureBindings_ObjectTypes_ResultId",
                        column: x => x.ResultId,
                        principalSchema: "dbo",
                        principalTable: "ObjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureParameterDescriptors",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Order = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Type = table.Column<string>(maxLength: 255, nullable: false),
                    ProcedureId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureParameterDescriptors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcedureParameterDescriptors_ProcedureDescriptors_ProcedureId",
                        column: x => x.ProcedureId,
                        principalSchema: "dbo",
                        principalTable: "ProcedureDescriptors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureResultDescriptors",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Order = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Type = table.Column<string>(maxLength: 255, nullable: false),
                    ProcedureId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureResultDescriptors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcedureResultDescriptors_ProcedureDescriptors_ProcedureId",
                        column: x => x.ProcedureId,
                        principalSchema: "dbo",
                        principalTable: "ProcedureDescriptors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionParameterBindings",
                schema: "dbo",
                columns: table => new
                {
                    ActionId = table.Column<long>(nullable: false),
                    RequestId = table.Column<long>(nullable: false),
                    ControllerRequestId = table.Column<long>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    IsFromUrl = table.Column<bool>(nullable: false),
                    IsFromBody = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionParameterBindings", x => new { x.ActionId, x.RequestId });
                    table.ForeignKey(
                        name: "FK_ActionParameterBindings_ControllerActions_ActionId",
                        column: x => x.ActionId,
                        principalSchema: "dbo",
                        principalTable: "ControllerActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionParameterBindings_ObjectTypes_ControllerRequestId",
                        column: x => x.ControllerRequestId,
                        principalSchema: "dbo",
                        principalTable: "ObjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureParameterBindings",
                schema: "dbo",
                columns: table => new
                {
                    ParameterId = table.Column<long>(nullable: false),
                    PropertyId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureParameterBindings", x => new { x.ParameterId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_ProcedureParameterBindings_ProcedureParameterDescriptors_ParameterId",
                        column: x => x.ParameterId,
                        principalSchema: "dbo",
                        principalTable: "ProcedureParameterDescriptors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcedureParameterBindings_ObjectProperties_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "dbo",
                        principalTable: "ObjectProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureResultColumnBindings",
                schema: "dbo",
                columns: table => new
                {
                    ResultColumnId = table.Column<long>(nullable: false),
                    PropertyId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureResultColumnBindings", x => new { x.ResultColumnId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_ProcedureResultColumnBindings_ObjectProperties_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "dbo",
                        principalTable: "ObjectProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcedureResultColumnBindings_ProcedureResultDescriptors_ResultColumnId",
                        column: x => x.ResultColumnId,
                        principalSchema: "dbo",
                        principalTable: "ProcedureResultDescriptors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionParameterBindings_ControllerRequestId",
                schema: "dbo",
                table: "ActionParameterBindings",
                column: "ControllerRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ControllerActions_ControllerDescriptorId",
                schema: "dbo",
                table: "ControllerActions",
                column: "ControllerDescriptorId");

            migrationBuilder.CreateIndex(
                name: "IX_ControllerActions_ControllerResponseId",
                schema: "dbo",
                table: "ControllerActions",
                column: "ControllerResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectProperties_DeclaringTypeId",
                schema: "dbo",
                table: "ObjectProperties",
                column: "DeclaringTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectProperties_PropertyTypeId",
                schema: "dbo",
                table: "ObjectProperties",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureBindings_RequestId",
                schema: "dbo",
                table: "ProcedureBindings",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureBindings_ResultId",
                schema: "dbo",
                table: "ProcedureBindings",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureParameterBindings_PropertyId",
                schema: "dbo",
                table: "ProcedureParameterBindings",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureParameterDescriptors_ProcedureId",
                schema: "dbo",
                table: "ProcedureParameterDescriptors",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureResultColumnBindings_PropertyId",
                schema: "dbo",
                table: "ProcedureResultColumnBindings",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureResultDescriptors_ProcedureId",
                schema: "dbo",
                table: "ProcedureResultDescriptors",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_SqlObjectTypeMappings_ObjectTypeId",
                schema: "dbo",
                table: "SqlObjectTypeMappings",
                column: "ObjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SqlObjectTypeMappings_SqlType_IsNullable",
                schema: "dbo",
                table: "SqlObjectTypeMappings",
                columns: new[] { "SqlType", "IsNullable" },
                unique: true,
                filter: "[SqlType] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionParameterBindings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Compilations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProcedureBindings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProcedureParameterBindings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProcedureResultColumnBindings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SqlObjectTypeMappings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ControllerActions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProcedureParameterDescriptors",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ObjectProperties",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProcedureResultDescriptors",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Controllers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ObjectTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProcedureDescriptors",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "HiLo",
                schema: "dbo");
        }
    }
}
