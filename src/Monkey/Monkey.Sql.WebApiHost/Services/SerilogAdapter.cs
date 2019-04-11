using System;
using Monkey.Logging;

namespace Monkey.Sql.WebApiHost.Services
{
    public class SerilogAdapter : ILogger
    {
        private Serilog.ILogger _logger;

        public SerilogAdapter(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public void Info(string text, params object[] args)
        {
            _logger.Information(text, args);
        }

        public void Warn(string text, params object[] args)
        {
            _logger.Warning(text, args);
        }

        public void Error(Exception ex, string text, params object[] args)
        {
            _logger.Error(ex, text, args);
        }

        public void Debug(string text, params object[] args)
        {
            _logger.Debug(text, args);
        }
    }
}