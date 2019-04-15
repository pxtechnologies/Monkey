using Microsoft.EntityFrameworkCore;
using Monkey.Sql.Model.Core;

namespace Monkey.Sql.Model
{
    public class ProcedureResultColumnBindingMapping : EntityMappiong<ProcedureResultColumnBinding>
    {
        protected override void Configure()
        {
            HasKey(e => new
            {
                ResultColumnColumnId = e.ResultColumnId,
                ObjectPropertyId = e.PropertyId
            });

            HasOne(x => x.Property)
                .WithMany()
                .HasForeignKey(x => x.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            HasOne(x => x.ResultColumn)
                .WithMany()
                .HasForeignKey(x => x.ResultColumnId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}