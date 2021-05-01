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
    [Route("api/v{version:apiVersion}/services")]
    public class ServicesController : MainController
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceService _serviceService;
        private readonly IMapper _mapper;

        public ServicesController(IServiceRepository serviceRepository,
                                  IServiceService serviceService,
                                  IMapper mapper,
                                  INotifier notifier, IUser user) : base(notifier, user)
        {
            _serviceRepository = serviceRepository;
            _serviceService = serviceService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Service", "Get")]
        [HttpGet]
        public async Task<IEnumerable<ServiceViewModel>> GetAll()
        {
            var sd= _mapper.Map<IEnumerable<ServiceViewModel>>(await _serviceRepository.GetAllServices());
            return sd;
        }

        [ClaimsAuthorize("Service", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ServiceViewModel>> GetById(Guid id)
        {
            var service = _mapper.Map<ServiceViewModel>(await _serviceRepository.GetServiceDoctorsByServiceId(id));

            if (service == null) return NotFound();

            return service;
        }

        [ClaimsAuthorize("Service", "Create")]
        [HttpPost("createservice")]
        public async Task<ActionResult<ServiceViewModel>> Create(ServiceViewModel serviceViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _serviceService.Create(_mapper.Map<Service>(serviceViewModel));

            return serviceViewModel;
        }

        [ClaimsAuthorize("Service", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ServiceViewModel>> Update(Guid id, ServiceViewModel serviceViewModel)
        {
            if (id != serviceViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(serviceViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _serviceService.Update(_mapper.Map<Service>(serviceViewModel));

            return CustomResponse(serviceViewModel);
        }

        [ClaimsAuthorize("Service", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ServiceViewModel>> Delete(Guid id)

        {
            var serviceViewModel = await _serviceRepository.GetById(id);

            if (serviceViewModel == null) return NotFound();

            await _serviceService.Delete(_mapper.Map<Service>(serviceViewModel));

            return CustomResponse(serviceViewModel);
        }
    }
}
