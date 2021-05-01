using AutoMapper;
using Core.Api.Extensions;
using Core.Api.ViewModels;
using Core.Business.Intefaces;
using Core.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/inquiries")]
    [ApiController]
    public class InquiriesController : MainController
    {
        private readonly IInquiryRepository _inquiryRepository;
        private readonly IInquiryService _inquiryService;
        private readonly IDapperDbRepository _dapperDbRepository;
        private readonly IMapper _mapper;

        public InquiriesController(IInquiryService inquiryService,
                                 IInquiryRepository inquiryRepository,
                                 IDapperDbRepository dapperDbRepository,
                                 IMapper mapper,
                                 INotifier notifier, IUser user) : base(notifier, user)
        {
            _inquiryRepository = inquiryRepository;
            _inquiryService = inquiryService;
            _dapperDbRepository = dapperDbRepository;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Inquiry", "Get")]
        [HttpGet]
        public async Task<IEnumerable<InquiryViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<InquiryViewModel>>(await _inquiryRepository.GetAll());
        }

        [ClaimsAuthorize("Inquiry", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<InquiryViewModel>> GetById(Guid id)
        {
            var questions = _mapper.Map<InquiryViewModel>(await _inquiryRepository.GetQuestionsById(id));

            if (questions == null) return NotFound();

            return questions;
        }

        [HttpGet("combo-inquiries")]
        public async Task<IEnumerable<ComboViewModel>> ComboInquiries()
        {
            return _mapper.Map<IEnumerable<ComboViewModel>>(await _dapperDbRepository.GetAllInquiries());
        }

        [ClaimsAuthorize("Inquiry", "Create")]
        [HttpPost]
        public async Task<ActionResult<InquiryViewModel>> Create(InquiryViewModel inquiryViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _inquiryService.Create(_mapper.Map<Inquiry>(inquiryViewModel));

            return CustomResponse(inquiryViewModel);
        }

        [ClaimsAuthorize("Inquiry", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<InquiryViewModel>> Update(Guid id, InquiryViewModel inquiryViewModel)
        {
            if (id != inquiryViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(inquiryViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _inquiryService.Update(_mapper.Map<Inquiry>(inquiryViewModel));

            return CustomResponse(inquiryViewModel);
        }

        [ClaimsAuthorize("Inquiry", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<InquiryViewModel>> Delete(Guid id)
        {
            var inquiryViewModel = await _inquiryRepository.GetById(id);

            if (inquiryViewModel == null) return NotFound();

            await _inquiryService.Delete(_mapper.Map<Inquiry>(inquiryViewModel));

            return CustomResponse(inquiryViewModel);
        }
    }
}
