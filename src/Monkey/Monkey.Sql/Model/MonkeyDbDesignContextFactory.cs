using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Monkey.Sql.Model
{
    public class MonkeyDbDesignContextFactory : IDesignTimeDbContextFactory<MonkeyDbContext>
    {
        public MonkeyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MonkeyDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=Monkey;Trusted_Connection=True;");

            return new MonkeyDbContext(optionsBuilder.Options);
        }
    }
}