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
    [Route("api/v{version:apiVersion}/specialties")]
    public class SpecialtiesController : MainController
    {
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly ISpecialtyService _specialtyService;
        private readonly IDapperDbRepository _dapperDbRepository;
        private readonly IMapper _mapper;

        public SpecialtiesController(ISpecialtyRepository specialtyRepository,
                                     ISpecialtyService specialtyService,
                                     IDapperDbRepository dapperDbRepository,
                                     IMapper mapper,
                                     INotifier notifier, IUser user) : base(notifier, user)
        {
            _specialtyRepository = specialtyRepository;
            _specialtyService = specialtyService;
            _dapperDbRepository = dapperDbRepository;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Specialty", "Get")]
        [HttpGet]
        public async Task<IEnumerable<SpecialtyViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<SpecialtyViewModel>>(await _specialtyRepository.GetSubSpecialtiesSpecialties());
        }

        [ClaimsAuthorize("Specialty", "Get")]
        [HttpGet("get-all-parent-specialities")]
        public async Task<IEnumerable<SpecialtyViewModel>> GetAllParentSpecialities()
        {
            return _mapper.Map<IEnumerable<SpecialtyViewModel>>(await _specialtyRepository.GetAllParentSpecialities());
        }

        [ClaimsAuthorize("Specialty", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SpecialtyViewModel>> GetById(Guid id)
        {
            var specialtyViewModel = _mapper.Map<SpecialtyViewModel>(await _specialtyRepository.GetSubSpecialtiesBySpecialtyId(id));

            if (specialtyViewModel == null) return NotFound();

            return specialtyViewModel;
        }


        [ClaimsAuthorize("Specialty", "Create")]
        [HttpPost]
        public async Task<ActionResult<SpecialtyViewModel>> Create(SpecialtyViewModel specialtyViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _specialtyService.Create(_mapper.Map<Specialty>(specialtyViewModel));

            return CustomResponse(specialtyViewModel);
        }


        [ClaimsAuthorize("Specialty", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<SpecialtyViewModel>> Update(Guid id, SpecialtyViewModel specialtyViewModel)
        {
            if (id != specialtyViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(specialtyViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _specialtyService.Update(_mapper.Map<Specialty>(specialtyViewModel));

            return CustomResponse(specialtyViewModel);
        }

        [ClaimsAuthorize("Specialty", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<SpecialtyViewModel>> Delete(Guid id)
        {
            var specialtyViewModel = await _specialtyRepository.GetById(id);

            if (specialtyViewModel == null) return NotFound();

            await _specialtyService.Delete(id);

            return CustomResponse(specialtyViewModel);
        }


        [HttpGet("combo-parent-specialties")]
        public async Task<IEnumerable<ComboViewModel>> ComboParentSpecialties()
        {
            return _mapper.Map<IEnumerable<ComboViewModel>>(await _dapperDbRepository.GetAllParentSpecialties());
        }

        [HttpGet("combo-specialties")]
        public async Task<IEnumerable<ComboViewModel>> ComboSpecialties()
        {
            return _mapper.Map<IEnumerable<ComboViewModel>>(await _dapperDbRepository.GetAllSpecialties());
        }
    }
}
