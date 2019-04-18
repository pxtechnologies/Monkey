using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Monkey.Sql.Model.Core;

namespace Monkey.Sql.Model
{
    public class MonkeyDbContext : DbContext, IRepository
    {
        private string _schemaName;

        /* Sql Minimum Schema Information */
        public DbSet<ProcedureDescriptor> ProcedureDescriptors { get; set; }
        public DbSet<ProcedureParameterDescriptor> ProcedureParameterDescriptors { get; set; }
        public DbSet<ProcedureResultColumn> ProcedureResultDescriptors { get; set; }

        /* Sql Bindings to CQRS Model */
        public DbSet<ProcedureParameterBinding> ProcedureParameterBindings { get; set; }
        public DbSet<ProcedureResultColumnBinding> ProcedureResultColumnBindings { get; set; }
        public DbSet<ProcedureBinding> ProcedureBindings { get; set; }
        public DbSet<Query> Queries { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<Result> Results { get; set; }

        /* .net */
        public DbSet<ObjectProperty> ObjectProperties { get; set; }
        
        /* WebApi - By convention */
        public DbSet<ControllerRequest> ControllerRequests { get; set; }
        public DbSet<ControllerResponse> ControllerResponses { get; set; }

        public DbSet<ControllerDescriptor> Controllers { get; set; }
        public DbSet<ControllerActionDescriptor> ControllerActions { get; set; }
        public DbSet<ActionParameterBinding> ActionParameterBindings { get; set; }
        public DbSet<Compilation> Compilations { get; set; }
        public DbSet<SqlObjectTypeMapping> SqlObjectTypeMappings { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public MonkeyDbContext(DbContextOptions<MonkeyDbContext> options) : base(options)
        {
            _schemaName = "dbo";
        }
        public MonkeyDbContext(DbContextOptions<MonkeyDbContext> options, string schema) : base(options)
        {
            this._schemaName = schema;
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
           
            mb.HasSequence<long>("HiLo")
                .StartsAt(1000).IncrementsBy(100);
            mb.ForSqlServerUseSequenceHiLo("HiLo");

            var m = typeof(MonkeyDbContext).Assembly.GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && typeof(IEntityMapping).IsAssignableFrom(x))
                .Select(x => Activator.CreateInstance(x))
                .OfType<IEntityMapping>()
                .ToArray();

            foreach (var mapping in m)
            {
                mapping.Configure(mb);
            }

            mb.HasDefaultSchema(_schemaName);
            base.OnModelCreating(mb);
        }

        public IQueryable<TEntity> Query<TEntity>() where TEntity:class
        {
            return base.Set<TEntity>();
        }

        public async Task Add<TEntity>(TEntity entity) where TEntity : class
        {
            await base.AddAsync(entity);
        }

        public async Task Update<TEntity>(TEntity entity) where TEntity : class
        {
            base.Update(entity);
        }

        public async Task Remove<TEntity>(TEntity entity) where TEntity : class
        {
            base.Remove(entity);
        }

        public async Task CommitChanges()
        {
            await base.SaveChangesAsync();
        }
    }
}