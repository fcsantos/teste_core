using Autofac;
using System.Linq;
using Microsoft.AspNetCore.Http;
using ProjetoMRP.Paciente.Extensions;
using ProjetoMRP.Paciente.Services;
using ProjetoMRP.Paciente.Services.Handlers;
using ProjetoMRP.Paciente.ViewModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac.Extras.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;

namespace ProjetoMRP.Paciente.Configuration
{
    public sealed class ContainerConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<UserLoginViewModel>().AsSelf();

            builder.RegisterType<AspNetUser>().As<IAspNetUser>();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();

            builder.RegisterType<HttpClientAuthorizationDelegatingHandler>();

            IContainer container = builder.Build();

            AutofacServiceLocator asl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => asl);

        }
    }
}
