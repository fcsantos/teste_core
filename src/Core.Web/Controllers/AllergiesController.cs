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
    public class AllergiesController : MainController
    {
        private readonly IAllergyService _allergyService;
        private readonly IClinicalSummaryFacilitatorService _clinicalSummaryFacilitatorService;
        private readonly IStringLocalizer<Resources> _localizerGeneral;
        private readonly IStringLocalizer<AllergiesController> _localizer;

        public AllergiesController(IAllergyService allergyService,
                                   IClinicalSummaryFacilitatorService clinicalSummaryFacilitatorService,
                                   IStringLocalizer<Resources> localizerGeneral,
                                   IStringLocalizer<AllergiesController> localizer)
        {
            _allergyService = allergyService;
            _clinicalSummaryFacilitatorService = clinicalSummaryFacilitatorService;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;
        }

        [ClaimsAuthorize("Allergy", "Create")]
        [HttpPost]
        public async Task<IActionResult> Create(SummaryClinicalDetailViewModel summaryClinicalDetailViewModel)
        {
            HttpContext.Session.SetString("ActiveTab", "Allergy");

            var response = await _allergyService.Create(summaryClinicalDetailViewModel.Allergy);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            if (summaryClinicalDetailViewModel.IsNewFacilitator)
            {
                summaryClinicalDetailViewModel.Facilitator.Description = summaryClinicalDetailViewModel.Allergy.Description;
                var responsePathology = await _clinicalSummaryFacilitatorService.Create(summaryClinicalDetailViewModel.Facilitator);
            }         

            return RedirectToAction("Index", "SummaryClinicalDetail", new { id = summaryClinicalDetailViewModel.Allergy.PatientId });
        }

        [ClaimsAuthorize("Allergy", "Get")]
        [HttpPost]
        public ActionResult Detail(Guid id)
        {
            var allergyViewModel = _allergyService.GetById(id).Result;

            if (allergyViewModel == null) return NotFound();

            return Json(new SummaryClinicalDetailViewModel { Allergy = allergyViewModel });
        }

        [ClaimsAuthorize("Allergy", "Delete")]
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            HttpContext.Session.SetString("ActiveTab", "Allergy");

            var allergyViewModel = _allergyService.GetById(Id).Result;

            if (allergyViewModel == null) return NotFound();

            var response = _allergyService.Delete(Id).Result;

            if (response.Errors.Messages.Count() > 0)
                return Json(new DeleteResponseMessage { message = response.Errors.Messages.FirstOrDefault(), success = false });

            return Json(new DeleteResponseMessage { message = allergyViewModel.IsActive.Value ? _localizerGeneral["Registro inativado com sucesso"] : _localizerGeneral["Registro ativado com sucesso"], success = true });
        }
    }
}
