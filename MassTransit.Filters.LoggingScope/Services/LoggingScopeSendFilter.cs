using GreenPipes;
using MassTransit.Filters.LoggingScope.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MassTransit.Filters.LoggingScope.Constants;

namespace MassTransit.Filters.LoggingScope.Services
{
    internal class LoggingScopeSendFilter : IFilter<SendContext>
    {
        private readonly ILoggingScopeAccessor _scopeAccessor;

        public LoggingScopeSendFilter(ILoggingScopeAccessor scopeAccessor) => _scopeAccessor = scopeAccessor;

        public void Probe(ProbeContext context) => context.CreateFilterScope(nameof(LoggingScopeSendFilter));

        public async Task Send(SendContext context, IPipe<SendContext> next)
        {
            var scopes = new List<object>();

            _scopeAccessor.ScopeProvider?.ForEachScope((scope, state) => state.Add(scope), scopes);

            if (scopes.Any())
            {
                context.Headers.Set(LoggingScopeHeader, scopes);
            }

            await next.Send(context);
        }
    }
}
