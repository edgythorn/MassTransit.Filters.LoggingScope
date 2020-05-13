using GreenPipes;
using MassTransit.Filters.LoggingScope.Interfaces;
using System.Collections.Generic;

namespace MassTransit.Filters.LoggingScope.Services
{
    internal class LoggingScopeSendFilterSpecification : IPipeSpecification<SendContext>
    {
        private readonly ILoggingScopeAccessor _scopeAccessor;

        public LoggingScopeSendFilterSpecification(ILoggingScopeAccessor scopeAccessor) => _scopeAccessor = scopeAccessor;

        public IEnumerable<ValidationResult> Validate() { yield break; }

        public void Apply(IPipeBuilder<SendContext> builder) => builder.AddFilter(new LoggingScopeSendFilter(_scopeAccessor));
    }
}
