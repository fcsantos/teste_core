using System;
using System.Linq;
using Core.Web.Extensions;
using Core.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Core.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly AppSettings _appSettings;
        private readonly IAspNetUser _aspNetUser;

        public HomeController(IStringLocalizer<HomeController> localizer,
                              IOptions<AppSettings> appSettings,
                              IAspNetUser aspNetUser)
        {
            _localizer = localizer;
            _appSettings = appSettings.Value;
            _aspNetUser = aspNetUser;
        }

        [Authorize]
        public IActionResult Index()
        {
            if (_aspNetUser.GetUserRole("medico"))
                return RedirectToAction("Index", "Dashboard");

            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Message = _localizer["Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte."];
                modelErro.Title = _localizer["Ocorreu um erro!"];
                modelErro.ErroCode = id;
            }
            else if (id == 404)
            {
                modelErro.Message =
                    _localizer["A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte"];
                modelErro.Title = _localizer["Ops! Página não encontrada."];
                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Message = _localizer["Você não tem permissão para fazer isto."];
                modelErro.Title = _localizer["Acesso Negado"];
                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(404);
            }

            return View("Error", modelErro);
        }


        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(_appSettings.ExpirationHours) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
