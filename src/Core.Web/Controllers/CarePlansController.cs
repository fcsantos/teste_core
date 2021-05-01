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
    public class CarePlansController : MainController
    {
        private readonly ICarePlanService _carePlanService;
        private readonly IClinicalSummaryFacilitatorService _clinicalSummaryFacilitatorService;
        private readonly IStringLocalizer<Resources> _localizerGeneral;
        private readonly IStringLocalizer<CarePlansController> _localizer;

        public CarePlansController(ICarePlanService carePlanService,
                                   IClinicalSummaryFacilitatorService clinicalSummaryFacilitatorService,
                                   IStringLocalizer<Resources> localizerGeneral,
                                   IStringLocalizer<CarePlansController> localizer)
        {
            _carePlanService = carePlanService;
            _clinicalSummaryFacilitatorService = clinicalSummaryFacilitatorService;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;
        }

        [ClaimsAuthorize("CarePlan", "Create")]
        [HttpPost]
        public async Task<IActionResult> Create(SummaryClinicalDetailViewModel summaryClinicalDetailViewModel)
        {
            HttpContext.Session.SetString("ActiveTab", "CarePlan");

            var response = await _carePlanService.Create(summaryClinicalDetailViewModel.CarePlan);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            if (summaryClinicalDetailViewModel.IsNewFacilitator)
            {
                summaryClinicalDetailViewModel.Facilitator.Description = summaryClinicalDetailViewModel.CarePlan.Description;
                var responsePathology = await _clinicalSummaryFacilitatorService.Create(summaryClinicalDetailViewModel.Facilitator);
            }

            if (summaryClinicalDetailViewModel.CloneId != Guid.Empty)
            {
                var carePlanViewModel = _carePlanService.GetById(summaryClinicalDetailViewModel.CloneId).Result;

                if (carePlanViewModel == null) return NotFound();

                await _carePlanService.Delete(summaryClinicalDetailViewModel.CloneId);
            }

            return RedirectToAction("Index", "SummaryClinicalDetail", new { id = summaryClinicalDetailViewModel.CarePlan.PatientId });
        }

        [ClaimsAuthorize("CarePlan", "Get")]
        [HttpPost]
        public ActionResult Detail(Guid id)
        {
            var carePlanViewModel = _carePlanService.GetById(id).Result;

            if (carePlanViewModel == null) return NotFound();

            return Json(new SummaryClinicalDetailViewModel { CarePlan = carePlanViewModel });
        }

        [ClaimsAuthorize("CarePlan", "Delete")]
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            HttpContext.Session.SetString("ActiveTab", "CarePlan");

            var carePlanViewModel = _carePlanService.GetById(Id).Result;

            if (carePlanViewModel == null) return NotFound();

            var response = _carePlanService.Delete(Id).Result;

            if (response.Errors.Messages.Count() > 0)
                return Json(new DeleteResponseMessage { message = response.Errors.Messages.FirstOrDefault(), success = false });

            return Json(new DeleteResponseMessage { message = carePlanViewModel.IsActive.Value ? _localizerGeneral["Registro inativado com sucesso"] : _localizerGeneral["Registro ativado com sucesso"], success = true });
        }

        [ClaimsAuthorize("CarePlan", "GetByPacient")]
        public async Task<IActionResult> PatientCarePlans()
        {
            return View(await _carePlanService.GetAllCarePlansByPacientId());
        }

    }
}
