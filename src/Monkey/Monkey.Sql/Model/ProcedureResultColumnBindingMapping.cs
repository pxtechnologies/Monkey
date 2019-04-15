using Monkey.Sql.Model.Core;

namespace Monkey.Sql.Model
{
    public class ProcedureResultColumnBindingMapping : EntityMappiong<ProcedureResultColumnBinding>
    {
        protected override void Configure()
        {
            HasKey(e => new
            {
                e.ResultColumnColumnId,
                e.ObjectPropertyId
            });
        }
    }
}