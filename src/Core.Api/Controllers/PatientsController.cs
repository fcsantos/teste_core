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
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/patients")]
    public class PatientsController : MainController
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IPatientService _patientService;
        private readonly IDapperDbRepository _dapperDbRepository;
        private readonly IMapper _mapper;

        public PatientsController(IPatientRepository patientRepository, 
                                  IPatientService patientService,
                                  IDapperDbRepository dapperDbRepository,
                                  IMapper mapper,
                                  INotifier notifier, IUser user) : base(notifier, user)
        {
            _patientRepository = patientRepository;
            _patientService = patientService;
            _dapperDbRepository = dapperDbRepository;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Patient", "Get")]
        [HttpGet]
        public async Task<IEnumerable<PatientViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<PatientViewModel>>(await _patientRepository.GetAll());
        }

        [ClaimsAuthorize("Patient", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PatientViewModel>> GetById(Guid id)
        {
            var patientViewModel = _mapper.Map<PatientViewModel>(await _patientRepository.GetWithAddressById(id));

            if (patientViewModel == null) return NotFound();

            return patientViewModel;
        }

        [ClaimsAuthorize("Patient", "Get")]
        [HttpGet("get-by-userid/{id:guid}")]
        public async Task<ActionResult<PatientViewModel>> GetByUserId(Guid id)
        {
            var patientViewModel = _mapper.Map<PatientViewModel>(await _patientRepository.GetPatientByUserId(id.ToString()));

            if (patientViewModel == null) return NotFound();

            return patientViewModel;
        }

        [ClaimsAuthorize("Patient", "Create")]
        [HttpPost]
        public async Task<ActionResult<PatientViewModel>> Create(PatientViewModel patientViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _patientService.Create(_mapper.Map<Patient>(patientViewModel));

            return CustomResponse(patientViewModel);
        }

        [ClaimsAuthorize("Patient", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<PatientViewModel>> Update(Guid id, PatientViewModel patientViewModel)
        {
            if (id != patientViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(patientViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _patientService.Update(_mapper.Map<Patient>(patientViewModel));

            return CustomResponse(patientViewModel);
        }

        [ClaimsAuthorize("Patient", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<PatientViewModel>> Delete(Guid id)
        {
            var patientViewModel = await _patientRepository.GetById(id);

            if (patientViewModel == null) return NotFound();

            await _patientService.Delete(_mapper.Map<Patient>(patientViewModel));

            return CustomResponse(patientViewModel);
        }


        [ClaimsAuthorize("Patient", "Get")]
        [HttpGet("get-all-paged")]
        public async Task<PagedResult<Patient>> GetAllPatientsPaged([FromQuery] int ps = 3, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            return await _dapperDbRepository.GetAllPatientsPaged(ps, page, q);
        }


        [HttpGet("combo")]
        public async Task<IEnumerable<ComboViewModel>> Combo()
        {
            return _mapper.Map<IEnumerable<ComboViewModel>>(await _dapperDbRepository.GetAllPatients());
        }
    }
}
