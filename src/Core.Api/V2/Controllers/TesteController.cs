using Elmah.Io.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core.Api.Controllers;
using Core.Business.Intefaces;
using System;

namespace Core.Api.V2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/teste")]
    public class TesteController : MainController
    {
        private readonly ILogger _logger;

        public TesteController(INotifier notificador, 
                               IUser appUser,
                               ILogger<TesteController> logger) : base(notificador, appUser)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Valor()
        {

            //throw new Exception("Error");

            //try
            //{
            //    var i = 0;
            //    var result = 42 / i;
            //}
            //catch (DivideByZeroException e)
            //{
            //    e.Ship(HttpContext);
            //}

            //só podem ser utilizados durante o desenvolvimento (podendo ser utilizado o information)
            _logger.LogTrace("Log de Trace");
            _logger.LogDebug("Log de Debug");

            _logger.LogInformation("Log de Informação");
            
            //ex.: http 404
            _logger.LogWarning("Log de Aviso");

            _logger.LogError("Log de Erro");

            //ameaça a saúde ou a performace da aplicação
            _logger.LogCritical("Log de Problema Critico");

            return "Sou a V2";
        }
    }
}