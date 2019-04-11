using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Monkey.Sql.WebApiHost.Configuration
{
    public static class LoggerConfig
    {
        public static LoggerConfiguration Default()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.With(new ExceptionEnricher())
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [Level: {Level}] [Environment: {Env}] [CorrelationId: {CorrelationId}] {EscapedMessage} {EscapedException}{NewLine}");
        }
    }
    class ExceptionEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent.Exception == null)
                return;

            var logEventProperty = propertyFactory.CreateProperty("EscapedException", logEvent.Exception.ToString().Replace(System.Environment.NewLine, "\\r\\n"));
            logEvent.AddPropertyIfAbsent(logEventProperty);
        }
    }
}