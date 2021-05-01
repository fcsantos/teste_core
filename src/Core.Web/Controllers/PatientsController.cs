using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Web.Extensions;
using Core.Web.Models;
using Core.Web.Resource;
using Core.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Core.Web.Helpers;

namespace Core.Web.Controllers
{
    public class PatientsController : MainController
    {
        private readonly IPatientService _patientService;
        private readonly IStringLocalizer<PatientsController> _localizer;
        private readonly IStringLocalizer<Resources> _localizerGeneral;
        private readonly IAccountService _accountService;
        private readonly AppSettings _appSettings;

        public PatientsController(IPatientService patientService,
                                  IStringLocalizer<PatientsController> localizer,
                                  IStringLocalizer<Resources> localizerGeneral,
                                  IAccountService accountService,
                                  IOptions<AppSettings> appSettings)
        {
            _patientService = patientService;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;
            _accountService = accountService;
            _appSettings = appSettings.Value;
        }

        [ClaimsAuthorize("Patient", "Get")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _patientService.GetAll());
        }

        [ClaimsAuthorize("Patient", "Create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [ClaimsAuthorize("Patient", "Create")]
        [HttpPost]
        public async Task<IActionResult> Create(PatientViewModel patientViewModel)
        {
            var password = string.Empty;
#if DEBUG
            password = _appSettings.DefaultPassword;
#else
                password = PasswordGenerator.GenerateRandomPassword();
#endif

            var userRegister = new UserRegister { Email = patientViewModel.Email, Password = password, ConfirmPassword = password, Role = _appSettings.RolePatient, Name = patientViewModel.FirstName };
            var responseAuth = await _accountService.RegisterUser(userRegister);

            if (ResponseHasErrors(responseAuth.ResponseResult))
            {
                return View(patientViewModel);
            }
            else
            {
                patientViewModel.UserId = responseAuth.UserToken.Id;
                var response = await _patientService.Create(patientViewModel);

                if (ResponseHasErrors(response))
                {
                    await _patientService.DeleteUser(responseAuth.UserToken.Id);

                    return View(patientViewModel);
                }
            }

            return RedirectToAction("Index", "Patients");
        }

        [ClaimsAuthorize("Patient", "Update")]
        [HttpGet("{id:guid}/EditPatient")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var patientViewModel = await _patientService.GetById(id);

            if (patientViewModel == null) return NotFound();

            ViewData["EditPatientName"] = _localizer["Editar Paciente: {0}", patientViewModel.Document + " - " + patientViewModel.FirstName + " " + patientViewModel.LastName];

            return View(patientViewModel);
        }

        [ClaimsAuthorize("Patient", "Update")]
        [HttpPost]
        public async Task<IActionResult> EditPost(PatientViewModel patientViewModel)
        {
            var response = await _patientService.Update(patientViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            return RedirectToAction("Index", "Patients");
        }

        [ClaimsAuthorize("Patient", "Delete")]
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            var patientViewModel = _patientService.GetById(Id).Result;

            if (patientViewModel == null) return NotFound();

            var response = _patientService.Delete(Id).Result;

            if (response.Errors.Messages.Count() > 0)
                return Json(new DeleteResponseMessage { message = response.Errors.Messages.FirstOrDefault(), success = false });

            return Json(new DeleteResponseMessage { message = patientViewModel.IsActive.Value ? _localizerGeneral["Registro inativado com sucesso"] : _localizerGeneral["Registro ativado com sucesso"], success = true });
        }

    }
}
