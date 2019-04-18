using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Monkey.Extensions;
using Monkey.Logging;

namespace Monkey.Sql.Services
{
    public interface IDbChangeListener
    {
        void Start();
    }
    internal class DbChangeListener : IDbChangeListener
    {
        private readonly IServiceProvider _serviceProvider;

        public DbChangeListener(IServiceProvider serviceProvider, ILogger logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
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
                try
                {
                    using (_serviceProvider.Scope())
                    {
                        await _serviceProvider.GetService<IServiceMatadataLoader>().Load();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Compilation failed: {message}", ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        private ILogger _logger;
        private Thread _worker;
        public void Start()
        {
            _worker.Start();
        }
    }
}
