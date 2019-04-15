using Microsoft.EntityFrameworkCore;
using Monkey.Sql.Model.Core;

namespace Monkey.Sql.Model
{
    public class ProcedureParameterBindingMapping : EntityMappiong<ProcedureParameterBinding>
    {
        protected override void Configure()
        {
            HasKey(e => new
            {
                e.ParameterId,
                e.PropertyId
            });
            HasOne(x => x.Parameter)
                .WithMany()
                .HasForeignKey(x=>x.ParameterId)
                .OnDelete(DeleteBehavior.Restrict);

            HasOne(x => x.Property)
                .WithMany()
                .HasForeignKey(x=>x.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}