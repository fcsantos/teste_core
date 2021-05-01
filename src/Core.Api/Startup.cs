using AutoMapper;
using Core.Api.Configuration;
using Core.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Core.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (hostEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //identity
            services.AddIdentityConfig(Configuration);

            //resolve automaticamente pegando tudo que estiver dentro do assembly
            services.AddAutoMapper(typeof(Startup));

            services.AddApiConfig();

            services.AddSwaggerConfig();

            services.AddLoggingConfig(Configuration);

            //injeção de dependencia dos objetos e do contexto
            services.ResolveDependencies();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider, ILoggerFactory loggerFactory)
        {
            // Add log file
            loggerFactory.AddFile("Configuration/Logs/LogCore-{Date}.txt", fileSizeLimitBytes: 20 * 1024 * 1024); // The maximum log file size (20MB here)

            Initialize.SeedUserAdmin(Configuration.GetSection("Roles").Get<List<string>>(), Configuration, app.ApplicationServices, loggerFactory).Wait();

            Initialize.SeedControllersActions(app.ApplicationServices, loggerFactory, Configuration).Wait();

            app.UseSwaggerConfig(provider);

            app.UseApiConfig(env);

            app.UseLoggingConfiguration();
        }
    }
}
