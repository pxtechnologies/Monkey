using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Monkey.Extensions;
using Monkey.Logging;

namespace Monkey.Sql.Services
{
    public interface IDbChangeListener : IDisposable
    {
        void Start();
    }
    internal class DbChangeListener : IDbChangeListener
    {
        private readonly IServiceProvider _serviceProvider;
        private TimeSpan _interval;
        public DbChangeListener(IServiceProvider serviceProvider, ILogger logger, IConfiguration config)
        {
            _sync = new object();
            _serviceProvider = serviceProvider;
            _logger = logger;
            _worker = new Thread(OnRun);
            _worker.IsBackground = true;
            _interval = config.GetValue<TimeSpan>("DbChangeListener:Interval");
            if (_interval == default(TimeSpan)) _interval = TimeSpan.FromSeconds(10);
        }

        private void OnRun()
        {
            OnRunAsync();
        }

        private void OnRunAsync()
        {
            while (!_isDisposing)
            {
                try
                {

                    using (_serviceProvider.Scope())
                    {
                        _serviceProvider.GetService<IServiceMatadataLoader>().Load().GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Compilation failed: {message}", ex.Message);
                }

                lock (_sync)
                    Monitor.Wait(_sync, _interval);
            }

            _worker = null;
        }

        private bool _isDisposing = false;
        private object _sync;
        private ILogger _logger;
        private Thread _worker;
        public void Start()
        {
            _worker.Start();
        }

        public void Dispose()
        {
            var t = _worker;
            AppDomain.CurrentDomain.ClearDynamicWorkspace();
            if (t != null && t.ThreadState != ThreadState.Unstarted)
            {
                _isDisposing = true;
                lock (_sync)
                    Monitor.Pulse(_sync);
                
                t.Join();
            }
        }
    }
    
}
