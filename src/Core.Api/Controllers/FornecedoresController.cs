using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Core.Api.ViewModels;
using Core.Business.Intefaces;
using Core.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Core.Api.Extensions;

namespace Core.Api.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/fornecedores")]
    public class FornecedoresController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IAddressRepository _addressRepository;

        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository,
                                      IMapper mapper,
                                      IFornecedorService fornecedorService,
                                      IAddressRepository addressRepository,
                                      IProdutoService produtoService,
                                      INotifier notifier, IUser user) : base(notifier, user)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
            _fornecedorService = fornecedorService;
            _addressRepository = addressRepository;

            _produtoService = produtoService;
        }

        [ClaimsAuthorize("Fornecedor", "Get")]
        [HttpGet]
        public async Task<IEnumerable<FornecedorViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.GetAll());
        }

        [ClaimsAuthorize("Fornecedor", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> GetById(Guid id)
        {
            var fornecedor = await ObterFornecedorProdutosEndereco(id);

            if (fornecedor == null) return NotFound();

            return fornecedor;
        }


        [ClaimsAuthorize("Fornecedor", "Create")]
        [HttpPost]
        public async Task<ActionResult<FornecedorViewModel>> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.Create(_mapper.Map<Fornecedor>(fornecedorViewModel));

            return CustomResponse(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Update(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(fornecedorViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.Update(_mapper.Map<Fornecedor>(fornecedorViewModel));

            return CustomResponse(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Delete(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            await _fornecedorService.Delete(id);

            return CustomResponse(fornecedorViewModel);
        }

        [HttpGet("endereco/{id:guid}")]
        public async Task<AddressViewModel> ObterEnderecoPorId(Guid id)
        {
            return _mapper.Map<AddressViewModel>(await _addressRepository.GetById(id));
        }

        [ClaimsAuthorize("Fornecedor", "Update")]
        [HttpPut("endereco/{id:guid}")]
        public async Task<IActionResult> AtualizarEndereco(Guid id, AddressViewModel enderecoViewModel)
        {
            if (id != enderecoViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(enderecoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.UpdateEndereco(_mapper.Map<Address>(enderecoViewModel));

            return CustomResponse(enderecoViewModel);
        }

        private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }

        private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }
    }
}
