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
    [Route("api/v{version:apiVersion}/pathologies")]
    public class PathologiesController : MainController
    {
        private readonly IPathologyRepository _pathologyRepository;
        private readonly IPathologyService _pathologyService;
        private readonly IDapperDbRepository _dapperDbRepository;
        private readonly IMapper _mapper;

        public PathologiesController(IPathologyRepository pathologyRepository, 
                                     IPathologyService pathologyService,
                                     IDapperDbRepository dapperDbRepository,
                                     IMapper mapper,
                                     INotifier notifier, IUser user) : base(notifier, user)
        {
            _pathologyRepository = pathologyRepository;
            _pathologyService = pathologyService;
            _dapperDbRepository = dapperDbRepository;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Pathology", "Get")]
        [HttpGet]
        public async Task<IEnumerable<PathologyViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<PathologyViewModel>>(await _pathologyRepository.GetSubPathologiesPathologies());
        }

        [ClaimsAuthorize("Pathology", "Get")]
        [HttpGet("get-all-parent-pathologies")]
        public async Task<IEnumerable<PathologyViewModel>> GetAllParentPathologies()
        {
            return _mapper.Map<IEnumerable<PathologyViewModel>>(await _pathologyRepository.GetAllParentPathologies());
        }

        [ClaimsAuthorize("Pathology", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PathologyViewModel>> GetById(Guid id)
        {
            var pathologyViewModel = _mapper.Map<PathologyViewModel>(await _pathologyRepository.GetSubPathologiesByPathologyId(id));

            if (pathologyViewModel == null) return NotFound();

            return pathologyViewModel;
        }


        [ClaimsAuthorize("Pathology", "Create")]
        [HttpPost]
        public async Task<ActionResult<PathologyViewModel>> Create(PathologyViewModel pathologyViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _pathologyService.Create(_mapper.Map<Pathology>(pathologyViewModel));

            return CustomResponse(pathologyViewModel);
        }


        [ClaimsAuthorize("Pathology", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<PathologyViewModel>> Update(Guid id, PathologyViewModel pathologyViewModel)
        {
            if (id != pathologyViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(pathologyViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _pathologyService.Update(_mapper.Map<Pathology>(pathologyViewModel));

            return CustomResponse(pathologyViewModel);
        }

        [ClaimsAuthorize("Pathology", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<PathologyViewModel>> Delete(Guid id)
        {
            var pathologyViewModel = await _pathologyRepository.GetById(id);

            if (pathologyViewModel == null) return NotFound();

            await _pathologyService.Delete(id);

            return CustomResponse(pathologyViewModel);
        }

        [HttpGet("combo-pathologies")]
        public async Task<IEnumerable<ComboViewModel>> ComboPathologies()
        {
            return _mapper.Map<IEnumerable<ComboViewModel>>(await _dapperDbRepository.GetAllPathologies());
        }

        [HttpGet("combo-parent-pathologies")]
        public async Task<IEnumerable<ComboViewModel>> ComboParentPathologies()
        {
            return _mapper.Map<IEnumerable<ComboViewModel>>(await _dapperDbRepository.GetAllParentPathologies());
        }
    }
}
