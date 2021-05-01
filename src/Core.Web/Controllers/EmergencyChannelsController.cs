using Core.Web.Extensions;
using Core.Web.Models;
using Core.Web.Resource;
using Core.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Controllers
{
    public class EmergencyChannelsController : MainController
    {
        private readonly IEmergencyChannelService _emergencyChannelService;
        private readonly IStringLocalizer<EmergencyChannelsController> _localizer;
        private readonly IStringLocalizer<Resources> _localizerGeneral;

        public EmergencyChannelsController(IEmergencyChannelService emergencyChannelService,
                                  IStringLocalizer<EmergencyChannelsController> localizer,
                                  IStringLocalizer<Resources> localizerGeneral)
        {
            _emergencyChannelService = emergencyChannelService;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;
        }

        [ClaimsAuthorize("EmergencyChannel", "Get")]
        public async Task<IActionResult> Index()
        {
            return View(await _emergencyChannelService.GetAll());
        }

        [ClaimsAuthorize("EmergencyChannel", "Create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [ClaimsAuthorize("EmergencyChannel", "Create")]
        [HttpPost]
        public async Task<IActionResult> Create(EmergencyChannelViewModel emergencyChannelViewModel)
        {
            var response = await _emergencyChannelService.Create(emergencyChannelViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            return RedirectToAction("Index", "EmergencyChannels");
        }

        [ClaimsAuthorize("EmergencyChannel", "Update")]
        [HttpGet("{id:guid}/EditEmergencyChannel")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var emergencyChannelViewModel = await _emergencyChannelService.GetById(id);

            if (emergencyChannelViewModel == null) return NotFound();

            ViewData["EditEmergencyChannelName"] = _localizer["Editar serviço de ajuda: {0}", emergencyChannelViewModel.Name];

            return View(emergencyChannelViewModel);
        }

        [ClaimsAuthorize("EmergencyChannel", "Update")]
        [HttpPost]
        public async Task<IActionResult> EditPost(EmergencyChannelViewModel emergencyChannelViewModel)
        {
            var response = await _emergencyChannelService.Update(emergencyChannelViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            return RedirectToAction("Index", "EmergencyChannels");
        }

        [ClaimsAuthorize("EmergencyChannel", "Delete")]
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            var emergencyChannelViewModel = _emergencyChannelService.GetById(Id).Result;

            if (emergencyChannelViewModel == null) return NotFound();

            var response = _emergencyChannelService.Delete(Id).Result;

            if (response.Errors.Messages.Count() > 0)
                return Json(new DeleteResponseMessage { message = _localizerGeneral[response.Errors.Messages.FirstOrDefault()], success = false });

            return Json(new DeleteResponseMessage { message = emergencyChannelViewModel.IsActive ? _localizerGeneral["Registro inativado com sucesso"] : _localizerGeneral["Registro ativado com sucesso"], success = true });
        }
    }
}
