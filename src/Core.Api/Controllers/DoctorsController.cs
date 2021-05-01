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
    [Route("api/v{version:apiVersion}/doctors")]
    [ApiController]
    public class DoctorsController : MainController
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDoctorService _doctorService;
        private readonly IDapperDbRepository _dapperDbRepository;
        private readonly IMapper _mapper;

        public DoctorsController(IDoctorService doctorService, 
                                 IDoctorRepository doctorRepository,
                                 IDapperDbRepository dapperDbRepository,
                                 IMapper mapper,
                                 INotifier notifier, IUser user) : base(notifier, user)
        {
            _doctorService = doctorService;
            _doctorRepository = doctorRepository;
            _dapperDbRepository = dapperDbRepository;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Doctor", "Get")]
        [HttpGet]
        public async Task<IEnumerable<DoctorViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<DoctorViewModel>>(await _doctorRepository.GetAllWithSpeciality());
        }

        [ClaimsAuthorize("Doctor", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DoctorViewModel>> GetById(Guid id)
        {
            var doctorViewModel = _mapper.Map<DoctorViewModel>(await _doctorRepository.GetDoctorWithSpecialityId(id));

            if (doctorViewModel == null) return NotFound();

            return doctorViewModel;
        }

        [ClaimsAuthorize("Doctor", "Get")]
        [HttpGet("get-by-userid/{id:guid}")]
        public async Task<ActionResult<DoctorViewModel>> GetByUserId(Guid id)
        {
            var doctorViewModel = _mapper.Map<DoctorViewModel>(await _doctorRepository.GetDoctorByUserId(id.ToString()));

            if (doctorViewModel == null) return NotFound();

            return doctorViewModel;
        }

        [ClaimsAuthorize("Doctor", "Create")]
        [HttpPost]
        public async Task<ActionResult<DoctorViewModel>> Create(DoctorViewModel doctorViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _doctorService.Create(_mapper.Map<Doctor>(doctorViewModel));

            return CustomResponse(doctorViewModel);
        }

        [ClaimsAuthorize("Doctor", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<DoctorViewModel>> Update(Guid id, DoctorViewModel doctorViewModel)
        {
            if (id != doctorViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(doctorViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _doctorService.Update(_mapper.Map<Doctor>(doctorViewModel));

            return CustomResponse(doctorViewModel);
        }

        [ClaimsAuthorize("Doctor", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<DoctorViewModel>> Delete(Guid id)
        {
            var doctorViewModel = await _doctorRepository.GetById(id);

            if (doctorViewModel == null) return NotFound();

            await _doctorService.Delete(_mapper.Map<Doctor>(doctorViewModel));

            return CustomResponse(doctorViewModel);
        }


        [HttpGet("combo")]
        public async Task<IEnumerable<ComboViewModel>> Combo()
        {
            return _mapper.Map<IEnumerable<ComboViewModel>>(await _dapperDbRepository.GetAllDoctors());
        }
    }
}
