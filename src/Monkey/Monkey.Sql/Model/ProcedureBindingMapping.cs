using Microsoft.EntityFrameworkCore;
using Monkey.Sql.Model.Core;

namespace Monkey.Sql.Model
{
    public class ProcedureBindingMapping : EntityMappiong<ProcedureBinding>
    {
        protected override void Configure()
        {
            HasKey(e => new
            {
                e.ProcedureId,
                e.ResultId
            });
            HasOne(x => x.Request)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            HasOne(x => x.Result)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}