using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Core.Web.Extensions;
using Core.Web.Models;
using Core.Web.Resource;
using Core.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Core.Web.Controllers
{
    public class DoctorsController : MainController
    {
        private readonly IDoctorService _doctorService;
        private readonly IStringLocalizer<DoctorsController> _localizer;
        private readonly IStringLocalizer<Resources> _localizerGeneral;
        private readonly IAccountService _accountService;
        private readonly AppSettings _appSettings;
        private readonly ISpecialtySevice _specialtySevice;

        public DoctorsController(IDoctorService doctorService,
                                 IStringLocalizer<DoctorsController> localizer,
                                 IStringLocalizer<Resources> localizerGeneral,
                                 IAccountService accountService,
                                 IOptions<AppSettings> appSettings,
                                 ISpecialtySevice specialtySevice)
        {
            _doctorService = doctorService;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;
            _accountService = accountService;
            _appSettings = appSettings.Value;
            _specialtySevice = specialtySevice;
        }

        [ClaimsAuthorize("Doctor", "Get")]
        public async Task<IActionResult> Index()
        {
            return View(await _doctorService.GetAll());
        }

        [ClaimsAuthorize("Doctor", "Create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["SystemSpecialties"] = _specialtySevice.GetAll().Result;
            return View();
        }

        [ClaimsAuthorize("Doctor", "Create")]
        [HttpPost]
        public async Task<IActionResult> Create(DoctorViewModel doctor)
        {
            var password = string.Empty;
            #if DEBUG
                password = _appSettings.DefaultPassword;
            #else
                password = PasswordGenerator.GenerateRandomPassword();
            #endif

            var userRegister = new UserRegister { Email = doctor.Email, Password = password, ConfirmPassword = password, Role = _appSettings.RoleDoctor, Name = doctor.Name };
            var responseAuth = await _accountService.RegisterUser(userRegister);

            if (ResponseHasErrors(responseAuth.ResponseResult))
            {
                TempData["Erros"] = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                ViewData["SystemSpecialties"] = _specialtySevice.GetAll().Result;
                return View(doctor);
            }
            else
            {
                doctor.UserId = responseAuth.UserToken.Id;
                var response = await _doctorService.Create(doctor);

                if (ResponseHasErrors(response))
                {
                    TempData["Erros"] = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                    ViewData["SystemSpecialties"] = _specialtySevice.GetAll().Result;
                    await _doctorService.DeleteUser(responseAuth.UserToken.Id);

                    return View(doctor);
                }
            }

            return RedirectToAction("Index", "Doctors");
        }

        [ClaimsAuthorize("Doctor", "Update")]
        [HttpGet("{id:guid}/EditDoctor")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var doctorViewModel = await _doctorService.GetById(id);

            if (doctorViewModel == null) return NotFound();

            ViewData["EditDoctorName"] = _localizer["Editar Médico: {0}", doctorViewModel.Name];

            return View(doctorViewModel);
        }

        [ClaimsAuthorize("Doctor", "Update")]
        [HttpPost]
        public async Task<IActionResult> EditPost(DoctorViewModel doctor)
        {
            var response = await _doctorService.Update(doctor);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            return RedirectToAction("Index", "Doctors");
        }

        [ClaimsAuthorize("Doctor", "Delete")]
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            var doctorViewModel = _doctorService.GetById(Id).Result;

            if (doctorViewModel == null) return NotFound();

            var response = _doctorService.Delete(Id).Result;

            if (response.Errors.Messages.Count() > 0)
                return Json(new DeleteResponseMessage { message = response.Errors.Messages.FirstOrDefault(), success = false });

            return Json(new DeleteResponseMessage { message = doctorViewModel.IsActive.Value ? _localizerGeneral["Registro inativado com sucesso"] : _localizerGeneral["Registro ativado com sucesso"], success = true });
        }
    }
}
