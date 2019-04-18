using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

            //Property(x => x.Mode)
            //    .HasConversion<EnumToStringConverter<Mode>>();
        }
    }
}