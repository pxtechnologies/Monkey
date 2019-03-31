using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Monkey.Sql.SimpleInjector
{
    public interface IMonkeyDatabase
    {
        Task Migrate();
        Task ReCreate();
    }

    class MonkeyDatabase : IMonkeyDatabase
    {
        private readonly DatabaseFacade _context;

        public MonkeyDatabase(DatabaseFacade context)
        {
            _context = context;
        }

        public async Task ReCreate()
        {
            await _context.EnsureDeletedAsync();
            await _context.MigrateAsync();
        }
        public async Task Migrate()
        {
            
            await _context.MigrateAsync();
        } 
    }
}