using GreenPipes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static MassTransit.Filters.LoggingScope.Constants;

namespace MassTransit.Filters.LoggingScope.Services
{
    internal class LoggingScopeConsumeFilter : IFilter<ConsumeContext>
    {
        private readonly ILogger _logger;

        public LoggingScopeConsumeFilter(ILogger logger) => _logger = logger;

        public void Probe(ProbeContext context) => context.CreateFilterScope(nameof(LoggingScopeConsumeFilter));

        public async Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
        {
            List<IDisposable> scopes = null;

            if (context.Headers.TryGetHeader(LoggingScopeHeader, out object header))
            {
                scopes = (header as JArray)
                    ?.Select(j => j.ToObject())
                    ?.Select(AsKeyValuePairIfPossible)
                    ?.Select(o => _logger.BeginScope(o))
                    ?.ToList();
            }
            try
            {
                await next.Send(context);
            }
            finally
            {
                scopes?.ForEach(s => s?.Dispose());
            }
        }

        private static object AsKeyValuePairIfPossible(object obj)
        {
            var collection = obj as IEnumerable<object>;
            if (collection != null && collection.Any())
            {
                var pairs = collection.OfType<KeyValuePair<string, object>>().ToList();
                if (pairs.Count == collection.Count())
                {
                    return pairs;
                }
            }
            return obj;
        }
    }
}
