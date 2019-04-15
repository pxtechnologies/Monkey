using Monkey.Sql.Model.Core;

namespace Monkey.Sql.Model
{
    public class SqlObjectTypeMappingMapping : EntityMappiong<SqlObjectTypeMapping>
    {
        protected override void Configure()
        {
            HasOne(x => x.ObjectType)
                .WithMany();
            HasIndex(x => new { x.SqlType, x.IsNullable })
                .IsUnique(true);
        }
    }
}