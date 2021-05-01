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
    public class DiagnosticsController : MainController
    {
        private readonly IDiagnosticService _diagnosticService;
        private readonly IClinicalSummaryFacilitatorService _clinicalSummaryFacilitatorService;
        private readonly IStringLocalizer<Resources> _localizerGeneral;
        private readonly IStringLocalizer<DiagnosticsController> _localizer;

        public DiagnosticsController(IDiagnosticService diagnosticService,
                                     IClinicalSummaryFacilitatorService clinicalSummaryFacilitatorService,
                                     IStringLocalizer<Resources> localizerGeneral,
                                     IStringLocalizer<DiagnosticsController> localizer)
        {
            _diagnosticService = diagnosticService;
            _clinicalSummaryFacilitatorService = clinicalSummaryFacilitatorService;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;
        }

        [ClaimsAuthorize("Diagnostic", "Create")]
        [HttpPost]
        public async Task<IActionResult> Create(SummaryClinicalDetailViewModel summaryClinicalDetailViewModel)
        {
            HttpContext.Session.SetString("ActiveTab", "Diagnostic");

            var response = await _diagnosticService.Create(summaryClinicalDetailViewModel.Diagnostic);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            if (summaryClinicalDetailViewModel.IsNewFacilitator)
            {
                summaryClinicalDetailViewModel.Facilitator.Description = summaryClinicalDetailViewModel.Diagnostic.Description;
                var responsePathology = await _clinicalSummaryFacilitatorService.Create(summaryClinicalDetailViewModel.Facilitator);
            }

            if (summaryClinicalDetailViewModel.CloneId != Guid.Empty)
            {
                var diagnosticViewModel = _diagnosticService.GetById(summaryClinicalDetailViewModel.CloneId).Result;

                if (diagnosticViewModel == null) return NotFound();

                await _diagnosticService.Delete(summaryClinicalDetailViewModel.CloneId);
            }

            return RedirectToAction("Index", "SummaryClinicalDetail", new { id = summaryClinicalDetailViewModel.Diagnostic.PatientId });
        }

        [ClaimsAuthorize("Diagnostic", "Get")]
        [HttpPost]
        public ActionResult Detail(Guid id)
        {
            var diagnosticViewModel = _diagnosticService.GetById(id).Result;

            if (diagnosticViewModel == null) return NotFound();

            return Json(new SummaryClinicalDetailViewModel { Diagnostic = diagnosticViewModel });
        }

        [ClaimsAuthorize("Diagnostic", "Delete")]
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            HttpContext.Session.SetString("ActiveTab", "Diagnostic");

            var diagnosticViewModel = _diagnosticService.GetById(Id).Result;

            if (diagnosticViewModel == null) return NotFound();

            var response = _diagnosticService.Delete(Id).Result;

            if (response.Errors.Messages.Count() > 0)
                return Json(new DeleteResponseMessage { message = response.Errors.Messages.FirstOrDefault(), success = false });

            return Json(new DeleteResponseMessage { message = diagnosticViewModel.IsActive.Value ? _localizerGeneral["Registro inativado com sucesso"] : _localizerGeneral["Registro ativado com sucesso"], success = true });
        }
    }
}
