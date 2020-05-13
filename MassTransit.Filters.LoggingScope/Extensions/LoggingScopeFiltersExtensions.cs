using GreenPipes;
using MassTransit.Filters.LoggingScope.Interfaces;
using MassTransit.Filters.LoggingScope.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace MassTransit
{
    public static class LoggingScopeFiltersExtensions
    {
        private static Lazy<ScopeLoggerProvider> _scopeLogger = new Lazy<ScopeLoggerProvider>(() => new ScopeLoggerProvider());

        /// <summary>
        /// Configure the pipeline to use a filter that adds current logging scope to the headers of sended message
        /// </summary>
        public static void UseSendLoggingScope(this ISendPipelineConfigurator configurator, IServiceProvider serviceProvider) =>
            configurator.ConfigureSend(cfg => cfg.AddPipeSpecification(
                new LoggingScopeSendFilterSpecification(serviceProvider.GetScopeAccessor())));

        /// <summary>
        /// Configure the pipeline to use a filter that adds current logging scope to the headers of published message
        /// </summary>
        public static void UsePublishLoggingScope(this IPublishPipelineConfigurator configurator, IServiceProvider serviceProvider) =>
            configurator.ConfigurePublish(cfg => cfg.AddPipeSpecification(
                new LoggingScopeSendFilterSpecification(serviceProvider.GetScopeAccessor())));

        /// <summary>
        /// Configure the pipeline to use a filter that extracts and sets logging scope from the headers of consumed message
        /// </summary>
        public static void UseConsumeLoggingScope(this IPipeConfigurator<ConsumeContext> configurator, IServiceProvider serviceProvider) =>
                configurator.AddPipeSpecification(
                    new LoggingScopeConsumeFilterSpecification(serviceProvider.GetRequiredService<ILogger<LoggingScopeConsumeFilter>>()));

        private static ILoggingScopeAccessor GetScopeAccessor(this IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddProvider(_scopeLogger.Value);
            return _scopeLogger.Value;
        }
    }
}
