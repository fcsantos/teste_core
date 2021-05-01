using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Web.Extensions;
using Core.Web.Models;
using Core.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using Core.Web.Resource;

namespace Core.Web.Controllers
{
    public class NoticesController : MainController
    {

        private readonly INoticeService _noticeService;
        private readonly IPatientService _patientService;
        private readonly IStringLocalizer<Resources> _localizerGeneral;
        private readonly IStringLocalizer<ServicesController> _localizer;

        public NoticesController(INoticeService noticeService,
                                    IPatientService patientService,
                                    IStringLocalizer<Resources> localizerGeneral,
                                    IStringLocalizer<ServicesController> localizer)
        {
            _noticeService = noticeService;
            _patientService = patientService;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;

        }

        [ClaimsAuthorize("Notice", "Get")]
        public async Task<IActionResult> Index()
        {
            return View(await _noticeService.GetAll());
        }

        [ClaimsAuthorize("Notice", "Create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["SelectListPatients"] = new SelectList(_patientService.Combo().Result, "Id", "Name");

            return View();
        }

        [ClaimsAuthorize("Notice", "Create")]
        [HttpPost]
        public async Task<IActionResult> Create(NoticeViewModel notice)
        {
            var response = await _noticeService.Create(notice);
            if (ResponseHasErrors(response))
            {
                TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

                ViewData["SelectListPatients"] = new SelectList(_patientService.Combo().Result, "Id", "Name");

                return View(notice);
            }

            return RedirectToAction("Index", "Notices");
        }

        [ClaimsAuthorize("Notice", "Update")]
        [HttpGet("{id:guid}/EditNotice")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var noticeViewModel = await _noticeService.GetById(id);

            if (noticeViewModel == null) return NotFound();

            ViewData["SelectListPatients"] = new SelectList(_patientService.Combo().Result, "Id", "Name");
            
            return View(noticeViewModel);
        }

        [HttpGet]
        public async Task<JsonResult> GetListPatients(Guid? noticeId)
        {
            
            //if (Guid.Empty == noticeId)
            //    return new JsonResult(null);

            var allPatients = await _patientService.Combo();
            var PatientsInNotice = await _noticeService.GetById(noticeId.Value);

            List<SelectListItem> listPatients = new List<SelectListItem>();

            foreach (var patient in allPatients)
            {
                var item = new SelectListItem
                {
                    Value = patient.Id.ToString(),
                    Text = patient.Name,
                    Selected = (PatientsInNotice != null && PatientsInNotice.NoticeUsers != null && PatientsInNotice.NoticeUsers.Contains(patient.Id))
                };

                listPatients.Add(item);
            }

            return Json(listPatients);
        }

        [ClaimsAuthorize("Notice", "Update")]
        [HttpPost]
        public async Task<IActionResult> EditPost(NoticeViewModel notice)
        {
            var response = await _noticeService.Update(notice);

            if (ResponseHasErrors(response))
            {
                TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

                return RedirectToAction("Edit", "Notices", new { id = notice.Id });
            }

            return RedirectToAction("Index", "Notices");
        }

        [ClaimsAuthorize("Notice", "Delete")]
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            var noticeViewModel = _noticeService.GetById(Id).Result;

            if (noticeViewModel == null) return NotFound();

            var response = _noticeService.Delete(Id).Result;

            if (response.Errors.Messages.Count() > 0)
                return Json(new DeleteResponseMessage { message = response.Errors.Messages.FirstOrDefault(), success = false });

            return Json(new DeleteResponseMessage { message = noticeViewModel.IsActive.Value ? _localizerGeneral["Registro inativado com sucesso"] : _localizerGeneral["Registro ativado com sucesso"], success = true });
        }

        [ClaimsAuthorize("Notice", "GetByPacient")]
        public async Task<IActionResult> IndexByPatient()
        {
            return View(await _noticeService.GetAllCurrentNoticeBy());
        }
    }
}