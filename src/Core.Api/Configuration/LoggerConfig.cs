using Elmah.Io.AspNetCore;
using Elmah.Io.AspNetCore.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.Api.Extensions;
using System;
using Elmah.Io.Extensions.Logging;
using Microsoft.Extensions.Logging;

namespace Core.Api.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddElmahIo(o =>
            {
                o.ApiKey = "da8ff36d13e441ed9dafe01d86136bd0";
                o.LogId = new Guid("d0b9e9ce-94a9-425c-864a-5719f53c0625");
            });

            services.AddLogging(builder =>
            {
                builder.AddElmahIo(o =>
                {
                    o.ApiKey = "da8ff36d13e441ed9dafe01d86136bd0";
                    o.LogId = new Guid("d0b9e9ce-94a9-425c-864a-5719f53c0625");
                });
                builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);
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