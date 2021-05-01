using Core.Web.Extensions;
using Core.Web.Models;
using Core.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Controllers
{
    public class ProfilesController : MainController
    {
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly IAspNetUser _aspNetUser;


        public ProfilesController(IPatientService patientService,
                                 IDoctorService doctorService,
                                 IAspNetUser aspNetUser,
                                 IStringLocalizer<AccountController> localizer)
        {
            _localizer = localizer;
            _patientService = patientService;
            _doctorService = doctorService;
            _aspNetUser = aspNetUser;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccountProfile()
        {
            var profile = User.Claims.Where(c => c.Type.ToLower().Equals("role")).ToArray();

            if (profile.Any(p => p.Value == "medico"))
                return RedirectToAction("DoctorProfile", "Profiles");

            else if (profile.Any(p => p.Value == "paciente"))
                return RedirectToAction("PatientProfile", "Profiles");

            return RedirectToAction("AdminProfile", "Profiles");
        }

        public async Task<IActionResult> PatientProfile()
        {
            var userId = Guid.Parse(User.GetUserId());
            var patientViewModel = await _patientService.GetByUserId(userId);

            if (patientViewModel == null) return NotFound();

            return View(patientViewModel);
        }

        public async Task<IActionResult> DoctorProfile()
        {
            var userId = Guid.Parse(User.GetUserId());
            var doctorViewModel = await _doctorService.GetByUserId(userId);

            if (doctorViewModel == null) return NotFound();

            return View(doctorViewModel);
        }

        public IActionResult AdminProfile()
        {
            var adminEmail = _aspNetUser.GetUserEmail();

            var userRegister = new UserRegister
            {
                Email = adminEmail
            };

            if (adminEmail == null) return NotFound();

            return View(userRegister);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientProfile(PatientViewModel patientViewModel)
        {
            patientViewModel.UserId = User.GetUserId();
            var patientViewModelOriginal = await _patientService.GetByUserId(Guid.Parse(User.GetUserId()));

            patientViewModel.Email = patientViewModelOriginal.Email;
            patientViewModel.CreatedBy = patientViewModelOriginal.CreatedBy;

            var response = await _patientService.Update(patientViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            return RedirectToAction("PatientProfile", "Profiles");
        }

        [HttpPost]
        public async Task<IActionResult> EditDoctorProfile(DoctorViewModel doctorViewModel)
        {
            doctorViewModel.UserId = User.GetUserId();
            var doctorViewModelOriginal = await _doctorService.GetByUserId(Guid.Parse(User.GetUserId()));

            doctorViewModel.Email = doctorViewModelOriginal.Email;

            var response = await _doctorService.Update(doctorViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            return RedirectToAction("DoctorProfile", "Profiles");
        }
    }
}
