using System;
using Core.Job.Services;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.Job
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseSqlServerStorage(Configuration["Hangfire"]));

            services.AddHangfireServer();

            services.AddSingleton<ISendMailToPatient, SendMailToPatient>();
            services.AddSingleton<IPrintJob, PrintJob>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Wellcome!!");
                });
            });

            app.UseHangfireDashboard("/JARVIS");

            backgroundJobClient.Enqueue(() => Console.WriteLine("Jobs - Run every minute!"));

            recurringJobManager.AddOrUpdate("Send Access to New Patient", () => serviceProvider.GetService<ISendMailToPatient>().SendAccessToNewPatientAsync(Configuration.GetSection("APICoreUrl").Value), "* * * * *");
            recurringJobManager.AddOrUpdate("Notification of new message to the Patient", () => serviceProvider.GetService<ISendMailToPatient>().NotificationOfNewMessageToPatientAsync(Configuration.GetSection("APICoreUrl").Value), "* * * * *");
            recurringJobManager.AddOrUpdate("Notification of inquiry schedule to the Patient", () => serviceProvider.GetService<ISendMailToPatient>().NotificationOfInquiryScheduleToPatient(Configuration.GetSection("APICoreUrl").Value), "* * * * *");
            recurringJobManager.AddOrUpdate("Job Sample", () => serviceProvider.GetService<IPrintJob>().Print(), "* * * * *");
        }
    }
}
