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
    public class PathologiesController : MainController
    {
        private readonly IPathologyService _pathologyService;
        private readonly IStringLocalizer<Resources> _localizerGeneral;
        private readonly IStringLocalizer<PathologiesController> _localizer;

        public PathologiesController(IPathologyService pathologyService, 
                                     IStringLocalizer<Resources> localizerGeneral, 
                                     IStringLocalizer<PathologiesController> localizer)
        {
            _pathologyService = pathologyService;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;
        }

        [ClaimsAuthorize("Pathology", "Get")]
        public async Task<IActionResult> Index()
        {
            return View(await _pathologyService.GetAll());
        }

        [ClaimsAuthorize("Pathology", "Create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["SelectListParentPathologies"] = new SelectList(_pathologyService.ComboParentPathologies().Result, "Id", "Name");

            return View();
        }

        [ClaimsAuthorize("Pathology", "Create")]
        [HttpPost]
        public async Task<IActionResult> Create(PathologyViewModel pathologyViewModel)
        {
            var response = await _pathologyService.Create(pathologyViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();


            return RedirectToAction("Index", "Pathologies");
        }

        [ClaimsAuthorize("Pathology", "Update")]
        [HttpGet("{id:guid}/EditPathology")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var pathologyViewModel = await _pathologyService.GetById(id);
            if (pathologyViewModel == null) return NotFound();

            ViewData["EditPathologyName"] = _localizer["Editar Patologia: {0}", pathologyViewModel.Name];
            ViewData["SelectListParentPathologies"] = new SelectList(_pathologyService.ComboParentPathologies().Result, "Id", "Name");

            return View(pathologyViewModel);
        }

        [ClaimsAuthorize("Pathology", "Update")]
        [HttpPost]
        public async Task<IActionResult> EditPost(PathologyViewModel pathologyViewModel)
        {
            var response = await _pathologyService.Update(pathologyViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();


            return RedirectToAction("Index", "Pathologies");
        }

        [ClaimsAuthorize("Pathology", "Delete")]
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            var pathologyViewModel =  _pathologyService.GetById(Id);
            if (pathologyViewModel == null) return NotFound();

            var response = _pathologyService.Delete(Id).Result;

            if (response.Errors.Messages.Count() > 0)
                return Json(new DeleteResponseMessage { message = _localizerGeneral[response.Errors.Messages.FirstOrDefault()], success = false });

            return Json(new DeleteResponseMessage { message = _localizerGeneral["Registro deletado com sucesso"], success = true });
        }
    }
}
