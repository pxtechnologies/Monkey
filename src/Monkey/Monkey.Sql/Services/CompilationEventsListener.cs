using System;
using System.Linq;
using System.Threading.Tasks;
using Monkey.Generator;
using Monkey.PubSub;

namespace Monkey.Sql.Services
{
    public class CompilationEventsListener
        :IEventSubscriber<AssemblyCompiledEvent>
    {
        private readonly IRepository _repo;

        public CompilationEventsListener(IRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(AssemblyCompiledEvent e)
        {
            await _repo.Add(new Model.Compilation()
            {
                Assembly = e.Data,
                Classes = string.Join(";",e.SourceCode.Select(x=>x.TypeName)),
                Purpose = e.Purpose,
                CompiledAt = e.When,
                CompilationDuration = e.Duration,
                Hash = e.SourceCode.SrcHash,
                ServerName = Environment.MachineName,
                Source = e.SourceCode.Code,
                Version = (int)e.SourceCode.Version
            });
            await _repo.CommitChanges();
        }
    }
}