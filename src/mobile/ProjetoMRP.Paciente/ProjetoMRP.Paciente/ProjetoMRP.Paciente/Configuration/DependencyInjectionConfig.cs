using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjetoMRP.Paciente.Extensions;
using ProjetoMRP.Paciente.Services;
using ProjetoMRP.Paciente.Services.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoMRP.Paciente.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            #region HttpServices

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            #endregion

        }
    }
}
