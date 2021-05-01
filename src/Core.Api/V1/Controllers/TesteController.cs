using Core.Api.Controllers;
using Core.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Core.Api.ViewModels;

namespace Core.Api.V1.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/teste")]
    public class TesteController : MainController
    {
        private readonly IConfiguration _configuration;

        public TesteController(INotifier notificador, IUser appUser, IConfiguration configuration) : base(notificador, appUser)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public string Valor()
        {
            var claims = _configuration.GetSection("ClaimsList").Get<Dictionary<string,string[]>>();

            return "Sou a V1";
        }
    }
}