using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Monkey.Sql.Model.Core;

namespace Monkey.Sql.Model
{
    public class WorkspaceMapping : EntityMappiong<Workspace>
    {
        protected override void Configure()
        {
            HasMany(x => x.Compilations).WithOne(x => x.Workspace);
            HasIndex(x => x.Status);
            //Property(x => x.Status)
            //    .HasConversion<EnumToStringConverter<WorkspaceStatus>>();
        }
    }
}