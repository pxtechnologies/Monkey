using Microsoft.EntityFrameworkCore;
using Monkey.Sql.Model.Core;

namespace Monkey.Sql.Model
{
    public class ObjectTypeMapping : EntityMappiong<ObjectType>
    {
        protected override void Configure()
        {
            Builder.HasDiscriminator<string>("Usage")
                .HasValue<Query>("Query")
                .HasValue<Command>("Command")
                .HasValue<Result>("Result")
                .HasValue<ControllerRequest>("Request")
                .HasValue<ControllerResponse>("Response")
                .HasValue<PrimitiveObject>("BCL");

            Property("Usage")
                .HasMaxLength(16);

            Builder.ToTable("ObjectTypes");
        }
    }
}