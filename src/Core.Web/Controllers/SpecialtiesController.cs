using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Web.Extensions;
using Core.Web.Models;
using Core.Web.Resource;
using Core.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace Core.Web.Controllers
{
    public class SpecialtiesController : MainController
    {
        private readonly ISpecialtySevice _specialtySevice;
        private readonly IStringLocalizer<Resources> _localizerGeneral;
        private readonly IStringLocalizer<SpecialtiesController> _localizer;

        public SpecialtiesController(ISpecialtySevice specialtySevice,
                                     IStringLocalizer<Resources> localizerGeneral,
                                     IStringLocalizer<SpecialtiesController> localizer)
        {
            _specialtySevice = specialtySevice;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;
        }

        [ClaimsAuthorize("Specialty", "Get")]
        public async Task<IActionResult> Index()
        {
            return View(await _specialtySevice.GetAll());
        }

        [ClaimsAuthorize("Specialty", "Create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["SelectListParentSpecialties"] = new SelectList(_specialtySevice.ComboParentSpecialties().Result, "Id", "Name");

            return View();
        }

        [ClaimsAuthorize("Specialty", "Create")]
        [HttpPost]
        public async Task<IActionResult> Create(SpecialtyViewModel specialtyViewModel)
        {
            var response = await _specialtySevice.Create(specialtyViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();


            return RedirectToAction("Index", "Specialties");
        }

        [ClaimsAuthorize("Specialty", "Update")]
        [HttpGet("{id:guid}/EditSpecialty")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var specialtyViewModel = await _specialtySevice.GetById(id);

            if (specialtyViewModel == null) return NotFound();

            ViewData["EditSpecialtyName"] = _localizer["Editar Especialidade: {0}", specialtyViewModel.Name];
            ViewData["SelectListParentSpecialties"] = new SelectList(_specialtySevice.ComboParentSpecialties().Result, "Id", "Name");

            return View(specialtyViewModel);
        }

        [ClaimsAuthorize("Specialty", "Update")]
        [HttpPost]
        public async Task<IActionResult> EditPost(SpecialtyViewModel specialtyViewModel)
        {
            var response = await _specialtySevice.Update(specialtyViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();


            return RedirectToAction("Index", "Specialties");
        }

        [ClaimsAuthorize("Specialty", "Delete")]
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            var specialtyViewModel = _specialtySevice.GetById(Id);

            if (specialtyViewModel == null) return NotFound();

            var response = _specialtySevice.Delete(Id).Result;

            if (response.Errors.Messages.Count() > 0)
                return Json(new DeleteResponseMessage { message = _localizerGeneral[response.Errors.Messages.FirstOrDefault()], success = false });

            return Json(new DeleteResponseMessage { message = _localizerGeneral["Registro deletado com sucesso"], success = true });
        }
    }
}
