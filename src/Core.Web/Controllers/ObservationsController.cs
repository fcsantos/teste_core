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
    public class ObservationsController : MainController
    {
        private readonly IObservationService _observationService;
        private readonly IClinicalSummaryFacilitatorService _clinicalSummaryFacilitatorService;
        private readonly IStringLocalizer<Resources> _localizerGeneral;
        private readonly IStringLocalizer<ObservationsController> _localizer;

        public ObservationsController(IObservationService observationService,
                                      IClinicalSummaryFacilitatorService clinicalSummaryFacilitatorService,
                                      IStringLocalizer<Resources> localizerGeneral,
                                      IStringLocalizer<ObservationsController> localizer)
        {
            _observationService = observationService;
            _clinicalSummaryFacilitatorService = clinicalSummaryFacilitatorService;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;
        }

        [ClaimsAuthorize("Observation", "Create")]
        [HttpPost]
        public async Task<IActionResult> Create(SummaryClinicalDetailViewModel summaryClinicalDetailViewModel)
        {
            HttpContext.Session.SetString("ActiveTab", "Observation");

            var response = await _observationService.Create(summaryClinicalDetailViewModel.Observation);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            if (summaryClinicalDetailViewModel.IsNewFacilitator)
            {
                summaryClinicalDetailViewModel.Facilitator.Description = summaryClinicalDetailViewModel.Observation.Description;
                var responsePathology = await _clinicalSummaryFacilitatorService.Create(summaryClinicalDetailViewModel.Facilitator);
            }

            if (summaryClinicalDetailViewModel.CloneId != Guid.Empty)
            {
                var observationViewModel = _observationService.GetById(summaryClinicalDetailViewModel.CloneId).Result;

                if (observationViewModel == null) return NotFound();

                await _observationService.Delete(summaryClinicalDetailViewModel.CloneId);
            }

            return RedirectToAction("Index", "SummaryClinicalDetail", new { id = summaryClinicalDetailViewModel.Observation.PatientId });
        }

        [ClaimsAuthorize("Observation", "Get")]
        [HttpPost]
        public ActionResult Detail(Guid id)
        {
            var observationViewModel = _observationService.GetById(id).Result;

            if (observationViewModel == null) return NotFound();

            return Json(new SummaryClinicalDetailViewModel { Observation = observationViewModel });
        }

        [ClaimsAuthorize("Observation", "Delete")]
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            HttpContext.Session.SetString("ActiveTab", "Observation");

            var observationViewModel = _observationService.GetById(Id).Result;

            if (observationViewModel == null) return NotFound();

            var response = _observationService.Delete(Id).Result;

            if (response.Errors.Messages.Count() > 0)
                return Json(new DeleteResponseMessage { message = response.Errors.Messages.FirstOrDefault(), success = false });

            return Json(new DeleteResponseMessage { message = observationViewModel.IsActive.Value ? _localizerGeneral["Registro inativado com sucesso"] : _localizerGeneral["Registro ativado com sucesso"], success = true });
        }
    }
}
