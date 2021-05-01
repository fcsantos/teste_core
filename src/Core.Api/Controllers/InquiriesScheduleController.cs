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
    [Route("api/v{version:apiVersion}/inquiries-schedule")]
    [ApiController]
    public class InquiriesScheduleController : MainController
    {
        private readonly IInquiryScheduleRepository _inquiryScheduleRepository;
        private readonly IInquiryScheduleService _inquiryScheduleService;

        private readonly IMapper _mapper;

        public InquiriesScheduleController(IInquiryScheduleRepository inquiryScheduleRepository,
                                           IInquiryScheduleService inquiryScheduleService,
                                           IMapper mapper,
                                           INotifier notifier, IUser user) : base(notifier, user)
        {
            _inquiryScheduleRepository = inquiryScheduleRepository;
            _inquiryScheduleService = inquiryScheduleService;
            _mapper = mapper;
        }


        [ClaimsAuthorize("Inquiry", "Get")]
        [HttpGet]
        public async Task<IEnumerable<InquiryScheduleViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<InquiryScheduleViewModel>>(await _inquiryScheduleRepository.GetAll());
        }

        [ClaimsAuthorize("Inquiry", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<InquiryScheduleViewModel>> GetById(Guid id)
        {
            var inquiryschedule = _mapper.Map<InquiryScheduleViewModel>(await _inquiryScheduleRepository.GetById(id));

            if (inquiryschedule == null) return NotFound();

            return inquiryschedule;
        }

        [ClaimsAuthorize("Inquiry", "Get")]
        [HttpGet("get-all-bydoctoruserid")]
        public async Task<IEnumerable<InquiryScheduleViewModel>> GetAllByDoctorUserId()
        {
            return _mapper.Map<IEnumerable<InquiryScheduleViewModel>>(await _inquiryScheduleRepository.GetAllByDoctorUserId());
        }

        [ClaimsAuthorize("Inquiry", "Get")]
        [HttpGet("get-all-bypatientuserid")]
        public async Task<IEnumerable<InquiryScheduleViewModel>> GetAllByPatientUserId()
        {
            return _mapper.Map<IEnumerable<InquiryScheduleViewModel>>(await _inquiryScheduleRepository.GetAllByPatientUserId());
        }

        [ClaimsAuthorize("Inquiry", "Get")]
        [HttpGet("get-all-bypatientid/{id:guid}")]
        public async Task<IEnumerable<InquiryScheduleViewModel>> GetAllByPatientId(Guid id)
        {
            return _mapper.Map<IEnumerable<InquiryScheduleViewModel>>(await _inquiryScheduleRepository.GetAllByPatientId(id));
        }

        [ClaimsAuthorize("Inquiry", "Get")]
        [HttpGet("get-all-bypatient-answered-or-not/{answered:bool}")]
        public async Task<IEnumerable<InquiryScheduleViewModel>> GetAllByPatientAnsweredOrNot(bool answered)
        {
            return _mapper.Map<IEnumerable<InquiryScheduleViewModel>>(await _inquiryScheduleRepository.GetAllByPatientAnsweredOrNot(answered));
        }

        [ClaimsAuthorize("Inquiry", "Get")]
        [HttpGet("get-all-answered-or-not/{answered:bool}")]
        public async Task<IEnumerable<InquiryScheduleViewModel>> GetAllByAnsweredOrNot(bool answered)
        {
            return _mapper.Map<IEnumerable<InquiryScheduleViewModel>>(await _inquiryScheduleRepository.GetAllByAnsweredOrNot(answered));
        }

        [ClaimsAuthorize("Inquiry", "Create")]
        [HttpPost]
        public async Task<ActionResult<InquiryScheduleViewModel>> Create(InquiryScheduleViewModel inquiryScheduleViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _inquiryScheduleService.CreateMany(_mapper.Map<InquirySchedule>(inquiryScheduleViewModel));
            return CustomResponse(inquiryScheduleViewModel);
        }


        [ClaimsAuthorize("Inquiry", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<InquiryScheduleViewModel>> Update(Guid id, InquiryScheduleViewModel inquiryScheduleViewModel)
        {
            if (id != inquiryScheduleViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(inquiryScheduleViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _inquiryScheduleService.Update(_mapper.Map<InquirySchedule>(inquiryScheduleViewModel));

            return CustomResponse(inquiryScheduleViewModel);
        }

        [ClaimsAuthorize("Inquiry", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<InquiryScheduleViewModel>> Delete(Guid id)
        {
            var inquiryScheduleViewModel = await _inquiryScheduleRepository.GetById(id);

            if (inquiryScheduleViewModel == null) return NotFound();

            await _inquiryScheduleService.Delete(_mapper.Map<InquirySchedule>(inquiryScheduleViewModel));

            return CustomResponse(inquiryScheduleViewModel);
        }
    }
}
