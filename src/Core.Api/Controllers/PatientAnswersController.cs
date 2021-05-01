using AutoMapper;
using Core.Api.Extensions;
using Core.Api.ViewModels;
using Core.Business.Intefaces;
using Core.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/patient-answers")]
    [ApiController]
    public class PatientAnswersController : MainController
    {
        private readonly IPatientAnswersRepository _patientAnswersRepository;
        private readonly IPatientAnswersService _patientAnswersService;
        private readonly IMapper _mapper;

        public PatientAnswersController(IPatientAnswersService patientAnswersService,
                                 IPatientAnswersRepository patientAnswersRepository,
                                 IMapper mapper,
                                 INotifier notifier, IUser user) : base(notifier, user)
        {
            _patientAnswersRepository = patientAnswersRepository;
            _patientAnswersService = patientAnswersService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("PatientAnswers", "Get")]
        [HttpGet]
        public async Task<IEnumerable<PatientAnswersViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<PatientAnswersViewModel>>(await _patientAnswersRepository.GetAll());
        }

        [ClaimsAuthorize("PatientAnswers", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PatientAnswersViewModel>> GetById(Guid id)
        {
            var patientAnswers = _mapper.Map<PatientAnswersViewModel>(await _patientAnswersRepository.GetById(id));

            if (patientAnswers == null) return NotFound();

            return patientAnswers;
        }

        [ClaimsAuthorize("PatientAnswers", "Get")]
        [HttpGet("answers-byinquiryschedule/{inquiryScheduleId:guid}")]
        public async Task<IEnumerable<PatientAnswersViewModel>> GetPatientAnswersByInquiryScheduleId(Guid inquiryScheduleId)
        {
            return _mapper.Map<IEnumerable<PatientAnswersViewModel>>(await _patientAnswersRepository.GetPatientAnswersByInquiryScheduleId(inquiryScheduleId));
        }

        [ClaimsAuthorize("PatientAnswers", "Create")]
        [HttpPost]
        public async Task<ActionResult<PatientAnswersViewModel>> Create(PatientAnswersViewModel patientAnswersViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _patientAnswersService.Create(_mapper.Map<PatientAnswers>(patientAnswersViewModel));

            return CustomResponse(patientAnswersViewModel);
        }

        [ClaimsAuthorize("PatientAnswers", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<PatientAnswersViewModel>> Update(Guid id, PatientAnswersViewModel patientAnswersViewModel)
        {
            if (id != patientAnswersViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(patientAnswersViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _patientAnswersService.Update(_mapper.Map<PatientAnswers>(patientAnswersViewModel));

            return CustomResponse(patientAnswersViewModel);
        }

        [ClaimsAuthorize("PatientAnswers", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<PatientAnswersViewModel>> Delete(Guid id)
        {
            var patientAnswersViewModel = await _patientAnswersRepository.GetById(id);

            if (patientAnswersViewModel == null) return NotFound();

            await _patientAnswersService.Delete(_mapper.Map<PatientAnswers>(patientAnswersViewModel));

            return CustomResponse(patientAnswersViewModel);
        }
    }
}
