using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Sql.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Controllers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Route = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controllers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObjectType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Namespace = table.Column<string>(maxLength: 255, nullable: true),
                    IsPrimitive = table.Column<bool>(nullable: false),
                    IsVoid = table.Column<bool>(nullable: false),
                    IsDynamic = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureParameterDescriptors",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Type = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureParameterDescriptors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureResultDescriptors",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Type = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureResultDescriptors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Procedures",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConnectionName = table.Column<string>(maxLength: 255, nullable: false),
                    Schema = table.Column<string>(maxLength: 255, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ControllerActions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                        principalTable: "Controllers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ControllerActions_ObjectType_ControllerResponseId",
                        column: x => x.ControllerResponseId,
                        principalTable: "ObjectType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ObjectProperties",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    PropertyTypeId = table.Column<long>(nullable: false),
                    IsCollection = table.Column<bool>(nullable: false),
                    DeclaringTypeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectProperties_ObjectType_DeclaringTypeId",
                        column: x => x.DeclaringTypeId,
                        principalTable: "ObjectType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ObjectProperties_ObjectType_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalTable: "ObjectType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureBindings",
                columns: table => new
                {
                    ProcedureId = table.Column<long>(nullable: false),
                    ResultId = table.Column<long>(nullable: false),
                    IsResultCollection = table.Column<bool>(nullable: false),
                    Mode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureBindings", x => new { x.ProcedureId, x.ResultId });
                    table.ForeignKey(
                        name: "FK_ProcedureBindings_Procedures_ProcedureId",
                        column: x => x.ProcedureId,
                        principalTable: "Procedures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcedureBindings_ObjectType_ResultId",
                        column: x => x.ResultId,
                        principalTable: "ObjectType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionParameterBindings",
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
                        principalTable: "ControllerActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionParameterBindings_ObjectType_ControllerRequestId",
                        column: x => x.ControllerRequestId,
                        principalTable: "ObjectType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureParameterBindings",
                columns: table => new
                {
                    ParameterId = table.Column<long>(nullable: false),
                    PropertyId = table.Column<long>(nullable: false),
                    ObjectPropertyId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureParameterBindings", x => new { x.ParameterId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_ProcedureParameterBindings_ObjectProperties_ObjectPropertyId",
                        column: x => x.ObjectPropertyId,
                        principalTable: "ObjectProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcedureParameterBindings_ProcedureParameterDescriptors_ParameterId",
                        column: x => x.ParameterId,
                        principalTable: "ProcedureParameterDescriptors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureResultColumnBindings",
                columns: table => new
                {
                    ResultColumnColumnId = table.Column<long>(nullable: false),
                    PropertyId = table.Column<long>(nullable: false),
                    ObjectPropertyId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureResultColumnBindings", x => new { x.ResultColumnColumnId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_ProcedureResultColumnBindings_ObjectProperties_ObjectPropertyId",
                        column: x => x.ObjectPropertyId,
                        principalTable: "ObjectProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcedureResultColumnBindings_ProcedureResultDescriptors_ResultColumnColumnId",
                        column: x => x.ResultColumnColumnId,
                        principalTable: "ProcedureResultDescriptors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionParameterBindings_ControllerRequestId",
                table: "ActionParameterBindings",
                column: "ControllerRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ControllerActions_ControllerDescriptorId",
                table: "ControllerActions",
                column: "ControllerDescriptorId");

            migrationBuilder.CreateIndex(
                name: "IX_ControllerActions_ControllerResponseId",
                table: "ControllerActions",
                column: "ControllerResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectProperties_DeclaringTypeId",
                table: "ObjectProperties",
                column: "DeclaringTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectProperties_PropertyTypeId",
                table: "ObjectProperties",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureBindings_ResultId",
                table: "ProcedureBindings",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureParameterBindings_ObjectPropertyId",
                table: "ProcedureParameterBindings",
                column: "ObjectPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureResultColumnBindings_ObjectPropertyId",
                table: "ProcedureResultColumnBindings",
                column: "ObjectPropertyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionParameterBindings");

            migrationBuilder.DropTable(
                name: "ProcedureBindings");

            migrationBuilder.DropTable(
                name: "ProcedureParameterBindings");

            migrationBuilder.DropTable(
                name: "ProcedureResultColumnBindings");

            migrationBuilder.DropTable(
                name: "ControllerActions");

            migrationBuilder.DropTable(
                name: "Procedures");

            migrationBuilder.DropTable(
                name: "ProcedureParameterDescriptors");

            migrationBuilder.DropTable(
                name: "ObjectProperties");

            migrationBuilder.DropTable(
                name: "ProcedureResultDescriptors");

            migrationBuilder.DropTable(
                name: "Controllers");

            migrationBuilder.DropTable(
                name: "ObjectType");
        }
    }
}
