using Microsoft.Extensions.DependencyInjection;

namespace DevIO.Api.Configuration
{
    public static class CorsConfig
    {
        public static IServiceCollection AddCorsConfig(this IServiceCollection services)
        {

            services.AddCors(options => {
                options.AddPolicy("Development",
                    builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()

                    .AllowCredentials()
                    .SetIsOriginAllowed(hostName => true));


                options.AddPolicy("Production",
                    builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://localhost:4200")
                    .AllowCredentials()
                    .SetIsOriginAllowed(hostName => true));
            });

            return services;
        }
    }
}
