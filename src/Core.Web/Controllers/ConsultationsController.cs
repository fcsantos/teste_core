using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Web.Extensions;
using Core.Web.Models;
using Core.Web.Resource;
using Core.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Core.Web.Controllers
{
    public class ConsultationsController : MainController
    {
        private readonly IConsultationService _consultationService;
        private readonly IStringLocalizer<ConsultationsController> _localizer;
        private readonly IStringLocalizer<Resources> _localizerGeneral;        

        public ConsultationsController(IConsultationService consultationService,
                                       IStringLocalizer<ConsultationsController> localizer,
                                       IStringLocalizer<Resources> localizerGeneral)
        {
            _consultationService = consultationService;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;            
        }

        [ClaimsAuthorize("Consultation", "Create")]
        [HttpPost]
        public async Task<IActionResult> Create(SummaryClinicalDetailViewModel summaryClinicalDetailViewModel)
        {
            HttpContext.Session.SetString("ActiveTab", "Consultation");

            var response = await _consultationService.Create(summaryClinicalDetailViewModel.Consultation);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();


            return RedirectToAction("Index", "SummaryClinicalDetail", new { id = summaryClinicalDetailViewModel.Consultation.PatientId });
        }

        [ClaimsAuthorize("Consultation", "Get")]
        [HttpPost]
        public ActionResult Detail(Guid id)
        {
            var consultationViewModel = _consultationService.GetById(id).Result;

            if (consultationViewModel == null) return NotFound();

            return Json(new SummaryClinicalDetailViewModel { Consultation = consultationViewModel });
        }
    }
}
