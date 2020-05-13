using Microsoft.Extensions.Logging;
using System;
using MassTransit.Filters.LoggingScope.Interfaces;

namespace MassTransit.Filters.LoggingScope.Services
{
    internal class ScopeLoggerProvider : ILoggerProvider, ISupportExternalScope, ILoggingScopeAccessor
    {
        public IExternalScopeProvider ScopeProvider { get; private set; }

        public ILogger CreateLogger(string categoryName) => new ScopeLogger(ScopeProvider);

        public void Dispose()
        { }

        public void SetScopeProvider(IExternalScopeProvider scopeProvider) => ScopeProvider = scopeProvider;

        private sealed class ScopeLogger : ILogger
        {
            private readonly IExternalScopeProvider _scopeProvider;

            public ScopeLogger(IExternalScopeProvider scopeProvider) => _scopeProvider = scopeProvider;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            { }

            public bool IsEnabled(LogLevel logLevel) => false;

            public IDisposable BeginScope<TState>(TState state) => _scopeProvider?.Push(state);
        }
    }
}
