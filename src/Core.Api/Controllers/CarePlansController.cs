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
    [Route("api/v{version:apiVersion}/careplans")]
    public class CarePlansController : MainController
    {
        private readonly ICarePlanRepository _carePlanRepository;
        private readonly ICarePlanService _carePlanService;
        private readonly IMapper _mapper;

        public CarePlansController(ICarePlanRepository carePlanRepository,
                                   ICarePlanService carePlanService,
                                   IMapper mapper,
                                   INotifier notifier, IUser user) : base(notifier, user)
        {
            _carePlanRepository = carePlanRepository;
            _carePlanService = carePlanService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("CarePlan", "GetByDoctor")]
        [HttpGet("get-all/{id:guid}")]
        public async Task<IEnumerable<CarePlanViewModel>> GetAll(Guid id)
        {
            return _mapper.Map<IEnumerable<CarePlanViewModel>>(await _carePlanRepository.GetAll(id));
        }

        [ClaimsAuthorize("CarePlan", "GetByDoctor")]
        [HttpGet("get-all-except-doctor/{id:guid}")]
        public async Task<IEnumerable<CarePlanViewModel>> GetAllExceptDoctor(Guid id)
        {
            return _mapper.Map<IEnumerable<CarePlanViewModel>>(await _carePlanRepository.GetAllExceptDoctor(id));
        }

        [ClaimsAuthorize("CarePlan", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CarePlanViewModel>> GetById(Guid id)
        {
            var carePlanViewModel = _mapper.Map<CarePlanViewModel>(await _carePlanRepository.GetById(id));

            if (carePlanViewModel == null) return NotFound();

            return carePlanViewModel;
        }

        [ClaimsAuthorize("CarePlan", "GetByPacient")]
        [HttpGet("get-all-careplans-bypatient")]
        public async Task<IEnumerable<CarePlanViewModel>> GetAllCarePlansByPacientId()
        {
            return _mapper.Map<IEnumerable<CarePlanViewModel>>(await _carePlanRepository.GetAllCarePlansByPacientId());
        }

        [ClaimsAuthorize("CarePlan", "Create")]
        [HttpPost]
        public async Task<ActionResult<CarePlanViewModel>> Create(CarePlanViewModel carePlanViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _carePlanService.Create(_mapper.Map<CarePlan>(carePlanViewModel));

            return CustomResponse(carePlanViewModel);
        }


        [ClaimsAuthorize("CarePlan", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CarePlanViewModel>> Update(Guid id, CarePlanViewModel carePlanViewModel)
        {
            if (id != carePlanViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(carePlanViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _carePlanService.Update(_mapper.Map<CarePlan>(carePlanViewModel));

            return CustomResponse(carePlanViewModel);
        }

        [ClaimsAuthorize("CarePlan", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CarePlanViewModel>> Delete(Guid id)
        {
            var carePlanViewModel = await _carePlanRepository.GetById(id);

            if (carePlanViewModel == null) return NotFound();

            await _carePlanService.Delete(_mapper.Map<CarePlan>(carePlanViewModel));

            return CustomResponse(carePlanViewModel);
        }
    }
}
