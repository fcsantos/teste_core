using Core.Web.Extensions;
using Core.Web.Models;
using Core.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Web.Controllers
{
    public class AccountController : MainController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountService _accountService;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly IStringLocalizer<AccountController> _localizer;

        public AccountController(IAuthenticationService authenticationService,
                                 IAccountService accountService,
                                 IPatientService patientService,
                                 IDoctorService doctorService,
                                 IStringLocalizer<AccountController> localizer)
        {
            _authenticationService = authenticationService;
            _accountService = accountService;
            _localizer = localizer;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        [ClaimsAuthorize("Account", "Register")]
        [HttpGet]
        [Route("new-account")]
        public IActionResult Register()
        {
            ViewData["SelectListRoles"] = new SelectList(_accountService.GetRoles(), "Name", "Name", null);

            return View();
        }

        [ClaimsAuthorize("Account", "Register")]
        [HttpPost]
        [Route("new-account")]
        public async Task<IActionResult> Register(UserRegister userRegister)
        {
            if (!ModelState.IsValid) return View(userRegister);

            if ("medico".Equals(userRegister.Role) || "paciente".Equals(userRegister.Role))
            {
                ModelState.AddModelError(string.Empty, "Não é permitido criar conta de Médico ou Paciente por este recurso!");
                TempData["Erros"] = TempData["Erros"] = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return View(userRegister);
            }
            var response = await _accountService.RegisterUser(userRegister);

            if (ResponseHasErrors(response.ResponseResult))
            {
                ViewData["SelectListRoles"] = new SelectList(_accountService.GetRoles(), "Name", "Name", null);
                TempData["Erros"] = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return View(userRegister);
            }

            //await _authenticationService.AccomplishLogin(response);            

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLogin userLogin, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(userLogin);

            var response = await _authenticationService.Login(userLogin);

            if (ResponseHasErrors(response.ResponseResult))
            {
                TempData["Erros"] =
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return View(userLogin);
            }

            await _authenticationService.AccomplishLogin(response);

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            foreach (var cookieKey in Request.Cookies.Keys) { Response.Cookies.Delete(cookieKey); }
            await _authenticationService.Logout();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Route("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            if (!ModelState.IsValid) return View(forgotPassword);

            var response = await _authenticationService.ForgotPassword(forgotPassword);

            if (ResponseHasErrors(response))
            {
                TempData["Erros"] =
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return View();
            }

            return RedirectToAction("Login", "Account");
        }


        [HttpGet]
        [Route("reset-password")]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null) return NotFound();

            return View(new ResetPasswordViewModel { Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)) });
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (!ModelState.IsValid) return View(resetPassword);

            var response = await _authenticationService.ResetPassword(resetPassword);

            if (ResponseHasErrors(response))
            {
                TempData["Erros"] =
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
            }

            return RedirectToAction("Login", "Account");
        }
    }
}