using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Sql.Migrations
{
    public partial class ExecuteCustomScripts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = typeof(ExecuteCustomScripts).Assembly;
            var names = assembly.GetManifestResourceNames()
                .Where(x => x.EndsWith(".sql"))
                .OrderBy(x=>x);

            foreach (var i in names)
            {
                using (Stream stream = assembly.GetManifestResourceStream(i))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string script = reader.ReadToEnd();
                    if(!string.IsNullOrWhiteSpace(script))
                        migrationBuilder.Sql(script, true);
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
