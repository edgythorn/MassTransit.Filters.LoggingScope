using GreenPipes;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace MassTransit.Filters.LoggingScope.Services
{
    class LoggingScopeConsumeFilterSpecification : IPipeSpecification<ConsumeContext>
    {
        private readonly ILogger _logger;

        public LoggingScopeConsumeFilterSpecification(ILogger logger) => _logger = logger;

        public IEnumerable<ValidationResult> Validate() { yield break; }

        public void Apply(IPipeBuilder<ConsumeContext> builder) => builder.AddFilter(new LoggingScopeConsumeFilter(_logger));
    }
}
