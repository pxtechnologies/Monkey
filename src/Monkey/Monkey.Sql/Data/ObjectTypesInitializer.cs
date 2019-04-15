using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Sql.Data
{
    static class ObjectTypesInitializer
    {
        public static void DeleteObjectTypes(this MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("ObjectTypes", "Id",ObjectTypeList.Instance.GetIds());
        }
        public static void InitObjectTypes(this MigrationBuilder migrationBuilder)
        {
            var data = ObjectTypeList.Instance.To2DArray();

            migrationBuilder.InsertData("ObjectTypes",
                new string[] { "Id", "Name", "Namespace", "Usage", "IsPrimitive", "Alias", "IsVoid", "IsDynamic" },
                data);
        }
    }
}
