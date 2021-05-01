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
using Core.Business.Models.DTO;

namespace Core.Api.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/notices")]
    public class NoticesController : MainController
    {
        private readonly INoticeRepository _noticeRepository;
        private readonly INoticeService _noticeService;
        private readonly INoticeUserRepository _noticeUserRepository;
        private readonly IMapper _mapper;

        public NoticesController(INoticeRepository noticeRepository,
                                 INoticeService noticeService,
                                 INoticeUserRepository noticeUserRepository,
                                 IMapper mapper,
                                 INotifier notifier, IUser user) : base(notifier, user)
        {
            _noticeRepository = noticeRepository;
            _noticeService = noticeService;
            _noticeUserRepository = noticeUserRepository;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Notice", "Get")]
        [HttpGet]
        public async Task<IEnumerable<NoticeViewModel>> GetAll()
        {            
            return _mapper.Map<IEnumerable<NoticeViewModel>>(await _noticeRepository.GetAllNotice()); ;
        }
     

        [ClaimsAuthorize("Notice", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<NoticeViewModel>> GetById(Guid id)
        {
            return _mapper.Map<NoticeViewModel>(await _noticeRepository.GetByNoticeId(id));
        }

        [ClaimsAuthorize("Notice", "Create")]
        [HttpPost]
        public async Task<ActionResult<NoticeViewModel>> Create(NoticeViewModel noticeViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            
            await _noticeService.Create(_mapper.Map<Notice>(noticeViewModel));
        
            return CustomResponse(noticeViewModel);
        }

        [ClaimsAuthorize("Notice", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<NoticeViewModel>> Update(Guid id, NoticeViewModel noticeViewModel)
        {
            if (id != noticeViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(noticeViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _noticeService.Update(_mapper.Map<Notice>(noticeViewModel));

            return CustomResponse(noticeViewModel);
        }

        [ClaimsAuthorize("Notice", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<NoticeViewModel>> Delete(Guid id)
        {
            var noticeViewModel = await _noticeRepository.GetById(id);

            if (noticeViewModel == null) return NotFound();

            await _noticeService.Delete(_mapper.Map<Notice>(noticeViewModel));

            return CustomResponse(noticeViewModel);
        }

        [ClaimsAuthorize("Notice", "GetByPacient")]
        [HttpGet("get-all-current-notice-by")]
        public async Task<IEnumerable<NoticeUserDto>> GetAllCurrentNoticeBy()
        {
            return await _noticeRepository.GetAllCurrentNoticeBy();
        }
    }
}
