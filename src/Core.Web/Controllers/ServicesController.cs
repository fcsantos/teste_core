using Core.Web.Extensions;
using Core.Web.Models;
using Core.Web.Resource;
using Core.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Controllers
{
    public class ServicesController : MainController
    {
        private readonly IServiceService _serviceSevice;
        private readonly IDoctorService _doctorSevice;
        private readonly IStringLocalizer<Resources> _localizerGeneral;
        private readonly IStringLocalizer<ServicesController> _localizer;

        public ServicesController(IServiceService serviceSevice,
                                     IDoctorService doctorSevice,
                                     IStringLocalizer<Resources> localizerGeneral,
                                     IStringLocalizer<ServicesController> localizer)
        {
            _serviceSevice = serviceSevice;
            _doctorSevice = doctorSevice;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;
        }

        [ClaimsAuthorize("Service", "Get")]
        public async Task<IActionResult> Index()
        {
            return View(await _serviceSevice.GetAll());
        }

        [ClaimsAuthorize("Service", "Get")]
        [HttpGet]
        public IActionResult Create()
        {
            var ListOfDoctors = _doctorSevice.GetAll().Result.ToList();

            ViewData["SelectListDoctors"] = new SelectList(_doctorSevice.Combo().Result, "Id", "Name");
            return View(new ServiceViewModel { ListOfDoctors = ListOfDoctors });
        }

        [ClaimsAuthorize("Service", "Create")]
        [HttpPost]
        public async Task<IActionResult> Create(ServiceViewModel serviceViewModel)
        {
            var response = await _serviceSevice.Create(serviceViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            return RedirectToAction("Index", "Services");
        }

        [ClaimsAuthorize("Service", "Update")]
        [HttpGet("{id:guid}/EditService")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var DoctorsInService = await _serviceSevice.GetById(id);
            if (DoctorsInService == null) return NotFound();
            ViewData["EditService"] = _localizer["Editar Serviço: {0}", DoctorsInService.ServiceName];

            ViewData["SelectListDoctors"] = new SelectList(_doctorSevice.Combo().Result, "Id", "Name");
            return View(DoctorsInService);
        }

        [HttpGet]
        public async Task<JsonResult> GetListDoctor(Guid serviceId)
        {
            var allDoctors = await _doctorSevice.Combo();
            var DoctorsInService = await _serviceSevice.GetById(serviceId);

            List<SelectListItem> listDoctor = new List<SelectListItem>();

            foreach (var doc in allDoctors)
            {
                var item = new SelectListItem
                {
                    Value = doc.Id.ToString(),
                    Text = doc.Name,
                    Selected = (DoctorsInService != null && DoctorsInService.ServiceDoctors != null && DoctorsInService.ServiceDoctors.Contains(doc.Id))
                };

                listDoctor.Add(item);
            }

            return Json(listDoctor);
        }

        [ClaimsAuthorize("Service", "Update")]
        [HttpPost]
        public async Task<IActionResult> EditPost(ServiceViewModel serviceViewModel)
        {
            var response = await _serviceSevice.Update(serviceViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            return RedirectToAction("Index", "Services");
        }

        [ClaimsAuthorize("Service", "Delete")]
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            var DoctorsInService = _serviceSevice.GetById(Id);
            if (DoctorsInService == null) return NotFound();

            var response = _serviceSevice.Delete(Id).Result;

            if (response.Errors.Messages.Count() > 0)
                return Json(new DeleteResponseMessage { message = response.Errors.Messages.FirstOrDefault(), success = false });

            return Json(new DeleteResponseMessage { message = _localizerGeneral["Registro deletado com sucesso"], success = true });
        }
    }
}
