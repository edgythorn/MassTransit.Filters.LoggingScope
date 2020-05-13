using Microsoft.Extensions.Logging;

namespace MassTransit.Filters.LoggingScope.Interfaces
{
    internal interface ILoggingScopeAccessor
    {
        IExternalScopeProvider ScopeProvider { get; }
    }
}
