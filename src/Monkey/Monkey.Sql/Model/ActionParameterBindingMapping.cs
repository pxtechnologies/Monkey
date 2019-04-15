using Monkey.Sql.Model.Core;

namespace Monkey.Sql.Model
{
    public class ActionParameterBindingMapping : EntityMappiong<ActionParameterBinding>
    {
        protected override void Configure()
        {
            HasKey(e =>
                new
                {
                    e.ActionId,
                    e.RequestId
                });
        }
    }
}