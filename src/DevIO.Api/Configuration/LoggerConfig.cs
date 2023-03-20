using Elmah.Io.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace DevIO.Api.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfig(this IServiceCollection services)
        {
            services.AddElmahIo(o =>
            {
                o.ApiKey = "11bc03a73c0d4c1cb5006f72bee2820f";
                o.LogId = new Guid("031e41f9-da23-4ae7-b99b-6e5de4b8faef");
            });

            services.AddLogging(builder => //configuração para capturar loggs feitos manualmente
            {
                builder.AddElmahIo(o =>
                {
                    o.ApiKey = "11bc03a73c0d4c1cb5006f72bee2820f";
                    o.LogId = new Guid("031e41f9-da23-4ae7-b99b-6e5de4b8faef");
                });

                builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning); //nivel de log (Tudo >= Warning)
            });

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            return app;
        }
    }
}
