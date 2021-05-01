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
    [Route("api/v{version:apiVersion}/messages")]
    public class MessagesController : MainController
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public MessagesController(IMessageRepository messageRepository,
                                  IMessageService messageService,
                                  IMapper mapper,                                                                    
                                  INotifier notifier, IUser user) : base(notifier, user)
        {
            _messageRepository = messageRepository;
            _messageService = messageService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Message", "GetByDoctor")]
        [HttpGet("get-all-messages-reply-bydoctor")]
        public async Task<IEnumerable<MessageViewModel>> GetAllMessagesReplyByDoctorId()
        {
            return _mapper.Map<IEnumerable<MessageViewModel>>(await _messageRepository.GetAllMessagesReplyByDoctorId());
        }

        [ClaimsAuthorize("Message", "GetByDoctor")]
        [HttpGet("get-all-messages-sent-bydoctor")]
        public async Task<IEnumerable<MessageViewModel>> GetAllMessagesSentByDoctorId()
        {
            return _mapper.Map<IEnumerable<MessageViewModel>>(await _messageRepository.GetAllMessagesSentByDoctorId());
        }

        [ClaimsAuthorize("Message", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MessageViewModel>> GetById(Guid id)
        {
            var messageViewModel = _mapper.Map<MessageViewModel>(await _messageRepository.GetById(id));

            if (messageViewModel == null) return NotFound();

            return messageViewModel;
        }

        [ClaimsAuthorize("Message", "GetBy")]
        [HttpGet("get-by-reply-message-id/{id:guid}")]
        public async Task<ActionResult<MessageViewModel>> GetByReplyMessageId(Guid id)
        {
            var messageViewModel = _mapper.Map<MessageViewModel>(await _messageRepository.GetByReplyMessageId(id));

            if (messageViewModel == null) return NotFound();

            return messageViewModel;
        }

        [ClaimsAuthorize("Message", "GetBy")]
        [HttpGet("get-by-id-with-replymessage/{id:guid}")]
        public async Task<ActionResult<MessageViewModel>> GetByIdWithReplyMessage(Guid id)
        {
            var messageViewModel = _mapper.Map<MessageViewModel>(await _messageRepository.GetByIdWithReplyMessage(id));

            if (messageViewModel == null) return NotFound();

            return messageViewModel;
        }


        [ClaimsAuthorize("Message", "GetByPacient")]
        [HttpGet("get-all-messages-bypatient")]
        public async Task<IEnumerable<MessageViewModel>> GetAllMessagesByPacientId()
        {
            return _mapper.Map<IEnumerable<MessageViewModel>>(await _messageRepository.GetAllMessagesByPacientId());
        }

        [ClaimsAuthorize("Message", "GetBy")]
        [HttpGet("get-all-messages-bypatientid/{id:guid}")]
        public async Task<IEnumerable<MessageViewModel>> GetAllMessagesByPacientId(Guid id)
        {
            return _mapper.Map<IEnumerable<MessageViewModel>>(await _messageRepository.GetAllMessagesByPacientId(id));
        }

        [ClaimsAuthorize("Message", "GetByPacient")]
        [HttpGet("get-all-not-read-messages-to-patient")]
        public async Task<IEnumerable<MessageViewModel>> GetAllNotReadMessagesToPatient()
        {
            return _mapper.Map<IEnumerable<MessageViewModel>>(await _messageRepository.GetAllNotReadMessagesToPatient());
        }

        [ClaimsAuthorize("Message", "GetByDoctor")]
        [HttpGet("get-all-not-read-messages-to-doctor")]
        public async Task<IEnumerable<MessageViewModel>> GetAllNotReadMessagesToDoctor()
        {
            return _mapper.Map<IEnumerable<MessageViewModel>>(await _messageRepository.GetAllNotReadMessagesToDoctor());
        }

        [ClaimsAuthorize("Message", "GetByPacient")]
        [HttpGet("get-all-awaiting-response-messages-to-patient")]
        public async Task<IEnumerable<MessageViewModel>> GetAllAwaitingResponseMessagesToPatient()
        {
            return _mapper.Map<IEnumerable<MessageViewModel>>(await _messageRepository.GetAllAwaitingResponseMessagesToPatient());
        }

        [ClaimsAuthorize("Message", "Create")]
        [HttpPost]
        public async Task<ActionResult<MessageViewModel>> Create(MessageViewModel messageViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _messageService.Create(_mapper.Map<Message>(messageViewModel));

            return CustomResponse(messageViewModel);
        }

        [ClaimsAuthorize("Message", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<MessageViewModel>> Update(Guid id, MessageViewModel messageViewModel)
        {
            if (id != messageViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(messageViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _messageService.Update(_mapper.Map<Message>(messageViewModel));

            return CustomResponse(messageViewModel);
        }

        [ClaimsAuthorize("Message", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<MessageViewModel>> Delete(Guid id)
        {
            var messageViewModel = await _messageRepository.GetById(id);

            if (messageViewModel == null) return NotFound();

            await _messageService.Delete(_mapper.Map<Message>(messageViewModel));

            return CustomResponse(messageViewModel);
        }
    }
}
