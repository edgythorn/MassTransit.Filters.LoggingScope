# Masstransit.Filters.LoggingScope
MassTransit middleware that allows to pass a logging scope from a producer to a consumer through the message headers.

Usage with MassTransit.Extensions.DependencyInjection:
``` csharp
            services.AddMassTransit(cfg =>
            {
                // (configure consumers)

                cfg.AddBus(sp => Bus.Factory.CreateUsingRabbitMq(buscfg =>
                {
                    // (configure host, endpoints, etc)
                        
                    buscfg.UseLoggingScope(sp.Container); // passes logging scope from a producer to a consumer
                }));
            });
```
or  
on publish/send side:
``` csharp
            services.AddMassTransit(cfg =>
                cfg.AddBus(sp => 
                    Bus.Factory.CreateUsingRabbitMq(buscfg =>
                    {
                        // (configure host)

                        buscfg.UseSendLoggingScope(sp.Container); //adds current logging scope to the headers of sended message
                        buscfg.UsePublishLoggingScope(sp.Container); //adds current logging scope to the headers of published/requested message
                    })));
```
on consume side:
``` csharp
            services.AddMassTransit(cfg =>
            {
                // (configure consumers)

                cfg.AddBus(sp => Bus.Factory.CreateUsingRabbitMq(buscfg =>
                {
                    // (configure host, endpoints, etc)

                    buscfg.UseConsumeLoggingScope(sp.Container); // extracts and sets logging scope from the headers of consumed message
                }));
            });
```
