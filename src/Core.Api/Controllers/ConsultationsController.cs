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
    [Route("api/v{version:apiVersion}/consultations")]
    public class ConsultationsController : MainController
    {
        private readonly IConsultationRepository _consultationRepository;
        private readonly IConsultationService _consultationService;
        private readonly IMapper _mapper;

        public ConsultationsController(IConsultationRepository consultationRepository,
                                       IConsultationService consultationService,
                                       IMapper mapper,
                                       INotifier notifier, IUser user) : base(notifier, user)
        {
            _consultationRepository = consultationRepository;
            _consultationService = consultationService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Consultation", "Get")]
        [HttpGet("get-all/{id:guid}")]
        public async Task<IEnumerable<ConsultationViewModel>> GetAll(Guid id)
        {
            return _mapper.Map<IEnumerable<ConsultationViewModel>>(await _consultationRepository.GetAll(id));
        }

        [ClaimsAuthorize("Consultation", "Get")]
        [HttpGet("get-all-except-doctor/{id:guid}")]
        public async Task<IEnumerable<ConsultationViewModel>> GetAllExceptDoctor(Guid id)
        {
            return _mapper.Map<IEnumerable<ConsultationViewModel>>(await _consultationRepository.GetAllExceptDoctor(id));
        }

        [ClaimsAuthorize("Consultation", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ConsultationViewModel>> GetById(Guid id)
        {
            var consultationViewModel = _mapper.Map<ConsultationViewModel>(await _consultationRepository.GetById(id));

            if (consultationViewModel == null) return NotFound();

            return consultationViewModel;
        }

        [ClaimsAuthorize("Consultation", "Create")]
        [HttpPost]
        public async Task<ActionResult<ConsultationViewModel>> Create(ConsultationViewModel consultationViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _consultationService.Create(_mapper.Map<Consultation>(consultationViewModel));

            return CustomResponse(consultationViewModel);
        }


        [ClaimsAuthorize("Consultation", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ConsultationViewModel>> Update(Guid id, ConsultationViewModel consultationViewModel)
        {
            if (id != consultationViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(consultationViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _consultationService.Update(_mapper.Map<Consultation>(consultationViewModel));

            return CustomResponse(consultationViewModel);
        }

        [ClaimsAuthorize("Consultation", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ConsultationViewModel>> Delete(Guid id)
        {
            var consultationViewModel = await _consultationRepository.GetById(id);

            if (consultationViewModel == null) return NotFound();

            await _consultationService.Delete(_mapper.Map<Consultation>(consultationViewModel));

            return CustomResponse(consultationViewModel);
        }
    }
}
