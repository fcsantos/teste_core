using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Api.Extensions;
using Core.Api.ViewModels;
using Core.Business.Intefaces;
using Core.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/clinical-sumary-facilitators")]
    public class ClinicalSummaryFacilitatorsController : MainController
    {
        private readonly IClinicalSummaryFacilitatorRepository _clinicalSummaryFacilitatorRepository;
        private readonly IClinicalSummaryFacilitatorService _clinicalSummaryFacilitatorService;
        private readonly IDapperDbRepository _dapperDbRepository;
        private readonly IMapper _mapper;

        public ClinicalSummaryFacilitatorsController(IClinicalSummaryFacilitatorRepository clinicalSummaryFacilitatorRepository,
                                                     IClinicalSummaryFacilitatorService clinicalSummaryFacilitatorService,
                                                     IDapperDbRepository dapperDbRepository,
                                                     IMapper mapper,
                                                     INotifier notifier, IUser user) : base(notifier, user)
        {
            _clinicalSummaryFacilitatorRepository = clinicalSummaryFacilitatorRepository;
            _clinicalSummaryFacilitatorService = clinicalSummaryFacilitatorService;
            _dapperDbRepository = dapperDbRepository;
            _mapper = mapper;
        }

        [ClaimsAuthorize("ClinicalSummaryFacilitator", "Get")]
        [HttpGet]
        public async Task<IEnumerable<ClinicalSummaryFacilitatorViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<ClinicalSummaryFacilitatorViewModel>>(await _clinicalSummaryFacilitatorRepository.GetAll());
        }

        [ClaimsAuthorize("ClinicalSummaryFacilitator", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ClinicalSummaryFacilitatorViewModel>> GetById(Guid id)
        {
            var clinicalSummaryFacilitatorViewModel = _mapper.Map<ClinicalSummaryFacilitatorViewModel>(await _clinicalSummaryFacilitatorRepository.GetById(id));

            if (clinicalSummaryFacilitatorViewModel == null) return NotFound();

            return clinicalSummaryFacilitatorViewModel;
        }

        [ClaimsAuthorize("ClinicalSummaryFacilitator", "Get")]
        [HttpGet("get-all-by-pathologyid/{id:guid}")]
        public async Task<IEnumerable<ClinicalSummaryFacilitatorViewModel>> GetAllByPathologyId(Guid id)
        {
            return _mapper.Map<IEnumerable<ClinicalSummaryFacilitatorViewModel>>(await _clinicalSummaryFacilitatorRepository.GetAllByPathologyId(id));
        }


        [ClaimsAuthorize("ClinicalSummaryFacilitator", "Create")]
        [HttpPost]
        public async Task<ActionResult<ClinicalSummaryFacilitatorViewModel>> Create(ClinicalSummaryFacilitatorViewModel clinicalSummaryFacilitatorViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _clinicalSummaryFacilitatorService.Create(_mapper.Map<ClinicalSummaryFacilitator>(clinicalSummaryFacilitatorViewModel));

            return CustomResponse(clinicalSummaryFacilitatorViewModel);
        }


        [ClaimsAuthorize("ClinicalSummaryFacilitator", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ClinicalSummaryFacilitatorViewModel>> Update(Guid id, ClinicalSummaryFacilitatorViewModel clinicalSummaryFacilitatorViewModel)
        {
            if (id != clinicalSummaryFacilitatorViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(clinicalSummaryFacilitatorViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _clinicalSummaryFacilitatorService.Update(_mapper.Map<ClinicalSummaryFacilitator>(clinicalSummaryFacilitatorViewModel));

            return CustomResponse(clinicalSummaryFacilitatorViewModel);
        }

        [ClaimsAuthorize("ClinicalSummaryFacilitator", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ClinicalSummaryFacilitatorViewModel>> Delete(Guid id)
        {
            var clinicalSummaryFacilitatorViewModel = await _clinicalSummaryFacilitatorRepository.GetById(id);

            if (clinicalSummaryFacilitatorViewModel == null) return NotFound();

            await _clinicalSummaryFacilitatorService.Delete(id);

            return CustomResponse(clinicalSummaryFacilitatorViewModel);
        }

        [HttpGet("combo/{type}")]
        public async Task<IEnumerable<ComboViewModel>> Combo(string type)
        {
            return _mapper.Map<IEnumerable<ComboViewModel>>(await _dapperDbRepository.GetAllClinicalSummaryFacilitator(type));
        }
    }
}
