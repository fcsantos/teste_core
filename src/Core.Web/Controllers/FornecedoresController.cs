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
    public class FornecedoresController : MainController
    {
        private readonly IFornecedorService _fornecedorService;
        private readonly IStringLocalizer<FornecedoresController> _localizer;
        private readonly IStringLocalizer<Resources> _localizerGeneral;

        public FornecedoresController(IFornecedorService fornecedorService,
                                      IStringLocalizer<FornecedoresController> localizer,
                                      IStringLocalizer<Resources> localizerGeneral)
        {
            _fornecedorService = fornecedorService;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;
        }

        [ClaimsAuthorize("Fornecedor", "Get")]
        public async Task<IActionResult> Index()
        {
            var teste = _localizer["teste"];
            var testeGeneral = _localizerGeneral["Bem vindo"];
            return View(await _fornecedorService.GetAll());
        }


        [ClaimsAuthorize("Fornecedor", "Create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [ClaimsAuthorize("Fornecedor", "Create")]
        [HttpPost]
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            var response = await _fornecedorService.Create(fornecedorViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] = 
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();


            return RedirectToAction("Index", "Fornecedores");
        }

        [ClaimsAuthorize("Fornecedor", "Update")]
        [HttpGet("{id:guid}/EditSupplier")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedorViewModel = await _fornecedorService.GetById(id);

            if (fornecedorViewModel == null) return NotFound();

            ViewData["EditSupplierName"] = _localizer["Editar Fornecedor {0}", fornecedorViewModel.Nome];

            return View(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor", "Update")]
        [HttpPost]
        public async Task<IActionResult> EditPost(FornecedorViewModel fornecedorViewModel)
        {            
            var response = await _fornecedorService.Update(fornecedorViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            return RedirectToAction("Index", "Fornecedores");
        }

        [ClaimsAuthorize("Fornecedor", "Delete")]
        [HttpPost]
        public ActionResult Delete(Guid Id)
        {
            var fornecedorViewModel =  _fornecedorService.GetById(Id);

            if (fornecedorViewModel == null) return NotFound();

            var response = _fornecedorService.Delete(Id).Result;

            if (response.Errors.Messages.Count() > 0)           
                return Json(new DeleteResponseMessage { message = response.Errors.Messages.FirstOrDefault(), success = false });            

            return Json(new DeleteResponseMessage { message = _localizerGeneral["Registro deletado com sucesso"], success = true });
        }
    }
}