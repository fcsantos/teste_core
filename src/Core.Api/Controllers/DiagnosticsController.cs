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
    [Route("api/v{version:apiVersion}/diagnostics")]
    public class DiagnosticsController : MainController
    {
        private readonly IDiagnosticRepository _diagnosticRepository;
        private readonly IDiagnosticService _diagnosticService;
        private readonly IMapper _mapper;

        public DiagnosticsController(IDiagnosticRepository diagnosticRepository,
                                    IDiagnosticService diagnosticService,
                                    IMapper mapper,
                                    INotifier notifier, IUser user) : base(notifier, user)
        {
            _diagnosticRepository = diagnosticRepository;
            _diagnosticService = diagnosticService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Diagnostic", "Get")]
        [HttpGet("get-all/{id:guid}")]
        public async Task<IEnumerable<DiagnosticViewModel>> GetAll(Guid id)
        {
            return _mapper.Map<IEnumerable<DiagnosticViewModel>>(await _diagnosticRepository.GetAll(id));
        }

        [ClaimsAuthorize("Diagnostic", "Get")]
        [HttpGet("get-all-except-doctor/{id:guid}")]
        public async Task<IEnumerable<DiagnosticViewModel>> GetAllExceptDoctor(Guid id)
        {
            return _mapper.Map<IEnumerable<DiagnosticViewModel>>(await _diagnosticRepository.GetAllExceptDoctor(id));
        }

        [ClaimsAuthorize("Diagnostic", "Get")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DiagnosticViewModel>> GetById(Guid id)
        {
            var diagnosticViewModel = _mapper.Map<DiagnosticViewModel>(await _diagnosticRepository.GetById(id));

            if (diagnosticViewModel == null) return NotFound();

            return diagnosticViewModel;
        }

        [ClaimsAuthorize("Diagnostic", "Create")]
        [HttpPost]
        public async Task<ActionResult<DiagnosticViewModel>> Create(DiagnosticViewModel diagnosticViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _diagnosticService.Create(_mapper.Map<Diagnostic>(diagnosticViewModel));

            return CustomResponse(diagnosticViewModel);
        }


        [ClaimsAuthorize("Diagnostic", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<DiagnosticViewModel>> Update(Guid id, DiagnosticViewModel diagnosticViewModel)
        {
            if (id != diagnosticViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(diagnosticViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _diagnosticService.Update(_mapper.Map<Diagnostic>(diagnosticViewModel));

            return CustomResponse(diagnosticViewModel);
        }

        [ClaimsAuthorize("Diagnostic", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<DiagnosticViewModel>> Delete(Guid id)
        {
            var diagnosticViewModel = await _diagnosticRepository.GetById(id);

            if (diagnosticViewModel == null) return NotFound();

            await _diagnosticService.Delete(_mapper.Map<Diagnostic>(diagnosticViewModel));

            return CustomResponse(diagnosticViewModel);
        }
    }
}
