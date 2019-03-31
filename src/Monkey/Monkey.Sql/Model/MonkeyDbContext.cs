using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public MonkeyDbContext(DbContextOptions<MonkeyDbContext> options) : base(options)
        {
            _schemaName = "dbo";
        }
        public MonkeyDbContext(DbContextOptions<MonkeyDbContext> options, string schema) : base(options)
        {
            this._schemaName = schema;
        }
        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<ProcedureParameterBinding>(x => x.HasKey(e => new
            {
                e.ParameterId,
                e.PropertyId
            }));
            mb.Entity<ProcedureResultColumnBinding>(x => x.HasKey(e => new
            {
                e.ResultColumnColumnId,
                e.PropertyId
            }));
            mb.Entity<ProcedureBinding>(x => x.HasKey(e => new
            {
                e.ProcedureId,
                e.ResultId
            }));
            mb.Entity<ActionParameterBinding>(x => x.HasKey(e =>
                new
                {
                    e.ActionId,
                    e.RequestId
                }));

            mb.Entity<ObjectProperty>().HasOne(x => x.DeclaringType)
                .WithMany(x => x.Properties)
                .OnDelete(DeleteBehavior.Restrict);

            mb.Entity<ObjectProperty>().HasOne(x => x.PropertyType)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            
            mb.Entity<ObjectType>().HasDiscriminator<string>("Usage")
                .HasValue<Query>("Query")
                .HasValue<Command>("Command")
                .HasValue<Result>("Result")
                .HasValue<ControllerRequest>("Request")
                .HasValue<ControllerResponse>("Response")
                .HasValue<PrimitiveObject>("Primitive");

            mb.Entity<ObjectType>()
                .Property("Usage")
                .HasMaxLength(16);

            
            mb.Entity<ObjectType>().ToTable("ObjectTypes");

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