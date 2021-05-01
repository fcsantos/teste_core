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
    [Route("api/v{version:apiVersion}/allergies")]
    public class AllergiesController : MainController
    {
        private readonly IAllergyRepository _allergyRepository;
        private readonly IAllergyService _allergyService;
        private readonly IMapper _mapper;

        public AllergiesController(IAllergyRepository allergyRepository,
                                   IAllergyService allergyService,
                                   IMapper mapper,                                   
                                   INotifier notifier, IUser user) : base(notifier, user)
        {
            _allergyRepository = allergyRepository;
            _allergyService = allergyService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Allergy", "Get")]
        [HttpGet("get-all/{id:guid}")]
        public async Task<IEnumerable<AllergyViewModel>> GetAll(Guid id)
        {
            return _mapper.Map<IEnumerable<AllergyViewModel>>(await _allergyRepository.GetAll(id));
        }

        [ClaimsAuthorize("Allergy", "Get")]
        [HttpGet("get-all-except-doctor/{id:guid}")]
        public async Task<IEnumerable<AllergyViewModel>> GetAllExceptDoctor(Guid id)
        {
            return _mapper.Map<IEnumerable<AllergyViewModel>>(await _allergyRepository.GetAllExceptDoctor(id));
        }

        [ClaimsAuthorize("Allergy", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AllergyViewModel>> GetById(Guid id)
        {
            var allergyViewModel = _mapper.Map<AllergyViewModel>(await _allergyRepository.GetById(id));

            if (allergyViewModel == null) return NotFound();

            return allergyViewModel;
        }

        [ClaimsAuthorize("Allergy", "Create")]
        [HttpPost]
        public async Task<ActionResult<AllergyViewModel>> Create(AllergyViewModel allergyViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _allergyService.Create(_mapper.Map<Allergy>(allergyViewModel));

            return CustomResponse(allergyViewModel);
        }


        [ClaimsAuthorize("Allergy", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<AllergyViewModel>> Update(Guid id, AllergyViewModel allergyViewModel)
        {
            if (id != allergyViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(allergyViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _allergyService.Update(_mapper.Map<Allergy>(allergyViewModel));

            return CustomResponse(allergyViewModel);
        }

        [ClaimsAuthorize("Allergy", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<AllergyViewModel>> Delete(Guid id)
        {
            var allergyViewModel = await _allergyRepository.GetById(id);

            if (allergyViewModel == null) return NotFound();

            await _allergyService.Delete(_mapper.Map<Allergy>(allergyViewModel));

            return CustomResponse(allergyViewModel);
        }
    }
}
