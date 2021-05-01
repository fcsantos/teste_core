using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Web.Models;
using Core.Web.Resource;
using Core.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace Core.Web.Controllers
{
    [Authorize]
    public class ManageUserClaimsController : MainController
    {
        private readonly IManageUserClaimsService _manageUserClaimsService;
        private readonly IStringLocalizer<ManageUserClaimsController> _localizer;
        private readonly IStringLocalizer<Resources> _localizerGeneral;

        public ManageUserClaimsController(IManageUserClaimsService manageUserClaimsService, 
                                          IStringLocalizer<ManageUserClaimsController> localizer, 
                                          IStringLocalizer<Resources> localizerGeneral)
        {
            _manageUserClaimsService = manageUserClaimsService;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _manageUserClaimsService.GetAllUsers());
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var ListUserClaims = await _manageUserClaimsService.GetClaimsByUser(id);
            if (ListUserClaims == null) return NotFound();          

            ViewData["EditUserClaims"] = _localizer["Editar Permissões: {0}", ListUserClaims.Count() > 0 ? ListUserClaims.FirstOrDefault().Name : _localizerGeneral["Utilizador"]];
            ViewData["SelectListControllers"] = new SelectList(_manageUserClaimsService.GetAllControllers().Result, "Id", "ControllerName");

            return View(new ControllerActionsViewModel { UserId = id.ToString(), ListOfClaims = ListUserClaims });
        }

        [HttpGet]
        public JsonResult GetActionsList(Guid userId, Guid controllerId, string controllerName)
        {
            var actionsByController = _manageUserClaimsService.GetActionsByController(controllerId).Result;
            var ListUserClaims = _manageUserClaimsService.GetClaimsByUser(userId).Result.ToList();

            var ListaFinal = actionsByController.Where(p => !ListUserClaims.Any(c => c.ClaimValue == p.ActionName && c.ClaimType == controllerName));

            var actionsList = new SelectList(ListaFinal, "ActionName", "ActionName");
            return Json(actionsList);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ControllerActionsViewModel controllerActionsViewModel)
        {
            var response = await _manageUserClaimsService.CreateClaims(controllerActionsViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            return RedirectToAction("Edit", "ManageUserClaims", new { id = controllerActionsViewModel.UserId });
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            var response = _manageUserClaimsService.Delete(Id).Result;

            if (response.Errors.Messages.Count() > 0)
                return Json(new DeleteResponseMessage { message = _localizerGeneral[response.Errors.Messages.FirstOrDefault()], success = false });

            return Json(new DeleteResponseMessage { message = _localizerGeneral["Registro deletado com sucesso"], success = true });
        }

    }
}