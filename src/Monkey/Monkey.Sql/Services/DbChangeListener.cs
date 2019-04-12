using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Monkey.Extensions;

namespace Monkey.Sql.Services
{
    public interface IDbChangeListener
    {
        void Start();
    }
    internal class DbChangeListener : IDbChangeListener
    {
        private readonly IServiceProvider _serviceProvider;

        public DbChangeListener(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _worker = new Thread(OnRun);
            _worker.IsBackground = true;
        }

        private void OnRun()
        {
            OnRunAsync().GetAwaiter().GetResult();
        }

        private async Task OnRunAsync()
        {
            while (true)
            {
                using (_serviceProvider.Scope())
                {
                    await _serviceProvider.GetService<IServiceMatadataLoader>().Load();
                }

                await Task.Delay(TimeSpan.FromMinutes(10));
            }
        }

        private Thread _worker;
        public void Start()
        {
            _worker.Start();
        }
    }
}
