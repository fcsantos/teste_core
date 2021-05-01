using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Web.Extensions;
using Core.Web.Models;
using Core.Web.Resource;
using Core.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Core.Web.Controllers
{
    public class ClinicalSummaryFacilitatorsController : MainController
    {
        private readonly IClinicalSummaryFacilitatorService _clinicalSummaryFacilitatorService;
        private readonly IPathologyService _pathologyService;
        private readonly IStringLocalizer<Resources> _localizerGeneral;
        private readonly IStringLocalizer<ClinicalSummaryFacilitatorsController> _localizer;

        public ClinicalSummaryFacilitatorsController(IClinicalSummaryFacilitatorService clinicalSummaryFacilitatorService,
                                                     IPathologyService pathologyService,
                                                     IStringLocalizer<Resources> localizerGeneral,
                                                     IStringLocalizer<ClinicalSummaryFacilitatorsController> localizer)
        {
            _clinicalSummaryFacilitatorService = clinicalSummaryFacilitatorService;
            _pathologyService = pathologyService;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;
        }

        [ClaimsAuthorize("ClinicalSummaryFacilitator", "Get")]
        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            var pathologyViewModel = await _pathologyService.GetById(id);

            if (pathologyViewModel == null) return NotFound();

            ViewData["PathologyId"] = id;
            ViewData["Facilitators"] = _localizer["Facilitador de Resumo Clínico da Patologia: {0}", pathologyViewModel.Name];

            return View(await _clinicalSummaryFacilitatorService.GetAllByPathologyId(id));
        }

        [ClaimsAuthorize("ClinicalSummaryFacilitator", "Create")]
        [HttpGet]
        public IActionResult Create(Guid id)
        {
            return View(new ClinicalSummaryFacilitatorViewModel { PathologyId = id });
        }

        [ClaimsAuthorize("ClinicalSummaryFacilitator", "Create")]
        [HttpPost]
        public async Task<IActionResult> Create(ClinicalSummaryFacilitatorViewModel clinicalSummaryFacilitatorViewModel)
        {
            clinicalSummaryFacilitatorViewModel.Id = Guid.Empty;
            var response = await _clinicalSummaryFacilitatorService.Create(clinicalSummaryFacilitatorViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();


            return RedirectToAction("Index", "ClinicalSummaryFacilitators", new { id = clinicalSummaryFacilitatorViewModel.PathologyId });
        }

        [ClaimsAuthorize("ClinicalSummaryFacilitator", "Update")]
        [HttpGet("{id:guid}/EditClinicalSummaryFacilitator")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var clinicalSummaryFacilitatorViewModel = await _clinicalSummaryFacilitatorService.GetById(id);

            if (clinicalSummaryFacilitatorViewModel == null) return NotFound();

            ViewData["EditClinicalSummaryFacilitatorName"] = _localizer["Editar Facilitador de Resumo Clínico {0}", clinicalSummaryFacilitatorViewModel.Name];

            return View(clinicalSummaryFacilitatorViewModel);
        }

        [ClaimsAuthorize("ClinicalSummaryFacilitator", "Update")]
        [HttpPost]
        public async Task<IActionResult> EditPost(ClinicalSummaryFacilitatorViewModel clinicalSummaryFacilitatorViewModel)
        {
            var response = await _clinicalSummaryFacilitatorService.Update(clinicalSummaryFacilitatorViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();


            return RedirectToAction("Index", "ClinicalSummaryFacilitators", new { id = clinicalSummaryFacilitatorViewModel.PathologyId });
        }

        [ClaimsAuthorize("ClinicalSummaryFacilitator", "Delete")]
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            var response = _clinicalSummaryFacilitatorService.Delete(Id).Result;

            if (response.Errors.Messages.Count() > 0)
                return Json(new DeleteResponseMessage { message = response.Errors.Messages.FirstOrDefault(), success = false });

            return Json(new DeleteResponseMessage { message = _localizerGeneral["Registro deletado com sucesso"], success = true });
        }
    }
}
