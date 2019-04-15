using Microsoft.EntityFrameworkCore;
using Monkey.Sql.Model.Core;

namespace Monkey.Sql.Model
{
    public class ObjectPropertyMapping : EntityMappiong<ObjectProperty>
    {
        protected override void Configure()
        {
            HasOne(x => x.DeclaringType)
                .WithMany(x => x.Properties)
                .OnDelete(DeleteBehavior.Restrict);

            HasOne(x => x.PropertyType)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}