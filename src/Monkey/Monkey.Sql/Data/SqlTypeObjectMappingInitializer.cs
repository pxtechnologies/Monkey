using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Sql.Data
{
    static class SqlTypeObjectMappingInitializer
    {
        public static void InitSqlObjectMappings(this MigrationBuilder mb)
        {
            var data = SqlObjectTypeList.Instance.To2DArray();
            mb.InsertData("SqlObjectTypeMappings",
                new string[] { "Id", "ObjectTypeId", "SqlType", "IsNullable" },
                data);
        }
        public static void DeleteSqlObjectMappings(this MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("SqlObjectTypeMappings", "Id", SqlObjectTypeList.Instance.GetIds());
        }
    }
}
