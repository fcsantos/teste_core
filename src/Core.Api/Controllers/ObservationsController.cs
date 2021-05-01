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
    [Route("api/v{version:apiVersion}/observations")]
    public class ObservationsController : MainController
    {
        private readonly IObservationRepository _observationRepository;
        private readonly IObservationService _observationService;
        private readonly IMapper _mapper;

        public ObservationsController(IObservationRepository observationRepository,
                                      IObservationService observationService,
                                      IMapper mapper,
                                      INotifier notifier, IUser user) : base(notifier, user)
        {
            _observationRepository = observationRepository;
            _observationService = observationService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Observation", "Get")]
        [HttpGet("get-all/{id:guid}")]
        public async Task<IEnumerable<ObservationViewModel>> GetAll(Guid id)
        {
            return _mapper.Map<IEnumerable<ObservationViewModel>>(await _observationRepository.GetAll(id));
        }

        [ClaimsAuthorize("Observation", "Get")]
        [HttpGet("get-all-except-doctor/{id:guid}")]
        public async Task<IEnumerable<ObservationViewModel>> GetAllExceptDoctor(Guid id)
        {
            return _mapper.Map<IEnumerable<ObservationViewModel>>(await _observationRepository.GetAllExceptDoctor(id));
        }

        [ClaimsAuthorize("Observation", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ObservationViewModel>> GetById(Guid id)
        {
            var observationViewModel = _mapper.Map<ObservationViewModel>(await _observationRepository.GetById(id));

            if (observationViewModel == null) return NotFound();

            return observationViewModel;
        }

        [ClaimsAuthorize("Observation", "Create")]
        [HttpPost]
        public async Task<ActionResult<ObservationViewModel>> Create(ObservationViewModel observationViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _observationService.Create(_mapper.Map<Observation>(observationViewModel));

            return CustomResponse(observationViewModel);
        }


        [ClaimsAuthorize("Observation", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ObservationViewModel>> Update(Guid id, ObservationViewModel observationViewModel)
        {
            if (id != observationViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(observationViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _observationService.Update(_mapper.Map<Observation>(observationViewModel));

            return CustomResponse(observationViewModel);
        }

        [ClaimsAuthorize("Observation", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ObservationViewModel>> Delete(Guid id)
        {
            var observationViewModel = await _observationRepository.GetById(id);

            if (observationViewModel == null) return NotFound();

            await _observationService.Delete(_mapper.Map<Observation>(observationViewModel));

            return CustomResponse(observationViewModel);
        }
    }
}
