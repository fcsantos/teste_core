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
    [Route("api/v{version:apiVersion}/EmergencyChannels")]
    [ApiController]
    public class EmergencyChannelController : MainController
    {
        private readonly IEmergencyChannelRepository _emergencyChannelRepository;
        private readonly IEmergencyChannelService _emergencyChannelService;
        private readonly IMapper _mapper;

        public EmergencyChannelController(IEmergencyChannelRepository emergencyChannelRepository,
            IEmergencyChannelService emergencyChannelService,
            IMapper mapper,
            INotifier notifier,
            IUser user) : base(notifier, user)
        {
            _emergencyChannelRepository = emergencyChannelRepository;
            _emergencyChannelService = emergencyChannelService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("EmergencyChannel", "Get")]
        [HttpGet]
        public async Task<IEnumerable<EmergencyChannelViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<EmergencyChannelViewModel>>(await _emergencyChannelRepository.GetAll());
        }


        [ClaimsAuthorize("EmergencyChannel", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmergencyChannelViewModel>> GetById(Guid id)
        {
            var emergencyChannelViewModel = _mapper.Map<EmergencyChannelViewModel>(await _emergencyChannelRepository.GetById(id));

            if (emergencyChannelViewModel == null) return NotFound();

            return emergencyChannelViewModel;
        }

        [ClaimsAuthorize("EmergencyChannel", "Create")]
        [HttpPost]
        public async Task<ActionResult<EmergencyChannelViewModel>> Create(EmergencyChannelViewModel emergencyChannelViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _emergencyChannelService.Create(_mapper.Map<EmergencyChannel>(emergencyChannelViewModel));

            return CustomResponse(emergencyChannelViewModel);
        }

        [ClaimsAuthorize("EmergencyChannel", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EmergencyChannelViewModel>> Update(Guid id, EmergencyChannelViewModel emergencyChannelViewModel)
        {
            if (id != emergencyChannelViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(emergencyChannelViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            emergencyChannelViewModel.IsActive = true;

            await _emergencyChannelService.Update(_mapper.Map<EmergencyChannel>(emergencyChannelViewModel));

            return CustomResponse(emergencyChannelViewModel);
        }

        [ClaimsAuthorize("EmergencyChannel", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<EmergencyChannelViewModel>> Delete(Guid id)
        {
            var emergencyChannelViewModel = await _emergencyChannelRepository.GetById(id);

            if (emergencyChannelViewModel == null) return NotFound();

            await _emergencyChannelService.Delete(emergencyChannelViewModel);

            return CustomResponse(emergencyChannelViewModel);
        }
    }
}
