using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Sql.Migrations
{
    public partial class Dictionary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alias",
                schema: "dbo",
                table: "ObjectTypes",
                maxLength: 32,
                nullable: true);

            migrationBuilder.InsertData("ObjectTypes",
                new string[] { "Id", "Name", "Namespace", "Usage", "IsPrimitive", "Alias", "IsVoid", "IsDynamic" },
                new object[,]
                {
                    {1,nameof(System.String),"System", "Primitive", 1, "string", 0 , 0},
                    {2,nameof(System.Int32),"System","Primitive", 1, "int" , 0, 0},
                    {3,nameof(System.Int64),"System","Primitive", 1, "long" , 0, 0},
                    {4,nameof(System.Double),"System","Primitive", 1, "double", 0, 0 },
                    {5,"float","System","Primitive", 1, "float", 0 , 0},
                    {6,nameof(System.DateTime),"System","Primitive", 1, "DateTime" , 0, 0},
                    {7,nameof(System.TimeSpan),"System","Primitive", 1, "TimeSpan", 0 , 0},
                    {8,nameof(System.DateTimeOffset),"System","Primitive", 1, "DateTimeOffset", 0, 0 },
                    {9,nameof(System.Decimal),"System","Primitive", 1, "Decimal" , 0, 0},

                    {100,"Nullable<Int32>","System","Primitive", 1, "int?" , 0, 0},
                    {101,"Nullable<Int64>","System","Primitive", 1, "long?" , 0, 0},
                    {102,"Nullable<Double>","System","Primitive", 1, "double?", 0, 0 },
                    {103,"Nullable<float>","System","Primitive", 1, "float?", 0 , 0},
                    {104,"Nullable<DateTime>","System","Primitive", 1, "DateTime?" , 0, 0},
                    {105,"Nullable<TimeSpan>","System","Primitive", 1, "TimeSpan?", 0 , 0},
                    {106,"Nullable<DateTimeOffset>","System","Primitive", 1, "DateTimeOffset?", 0, 0 },
                    {107,"Nullable<Decimal>","System","Primitive", 1, "Decimal?" , 0, 0}
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                schema: "dbo",
                table: "ObjectTypes");

            migrationBuilder.DeleteData("ObjectTypes","Id", 
                new object[]{ 1,2,3,4,5,6,7,8,9,100,101,102,103,104,105,106,107});
        }
    }
}
