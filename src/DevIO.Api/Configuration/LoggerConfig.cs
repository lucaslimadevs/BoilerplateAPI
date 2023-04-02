using DevIO.Api.Extensions;
using Elmah.Io.Extensions.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace DevIO.Api.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfig(this IServiceCollection services, IConfiguration configration)
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

            services.AddHealthChecks()
                .AddElmahIoPublisher(options =>
                {
                    options.ApiKey = "11bc03a73c0d4c1cb5006f72bee2820f";
                    options.LogId = new Guid("031e41f9-da23-4ae7-b99b-6e5de4b8faef");
                    options.HeartbeatId = "40339c257c434c4385c44a3343f8b375";
                    options.Application = "Boilerplate API";
                    options.OnHeartbeat = hb =>
                    {
                        hb.Version = "1.0.0";
                    };
                    options.OnFilter = hb =>
                    {
                        return hb.Result == "Degraded";
                    };
                })
                .AddCheck("Produtos", new SqlServerHealthCheck(configration.GetConnectionString("DefaultConnection")))
                .AddSqlServer(configration.GetConnectionString("DefaultConnection"), null, "Database");

            services.AddHealthChecksUI();

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            app.UseHealthChecks("/api/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/api/hc-ui";
            });

            return app;
        }
    }
}
