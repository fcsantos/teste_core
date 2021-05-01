using System;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class DiagnosticService : BaseService, IDiagnosticService
    {
        private readonly IDiagnosticRepository _diagnosticRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUser _user;

        public DiagnosticService(IDiagnosticRepository diagnosticRepository,
                                 IDoctorRepository doctorRepository,
                                 INotifier notifier, IUser user) : base(notifier)
        {
            _diagnosticRepository = diagnosticRepository;
            _doctorRepository = doctorRepository;
            _user = user;
        }

        public async Task Create(Diagnostic diagnostic)
        {
            if (!ExecuteValidation(new DiagnosticValidation(), diagnostic)) return;

            diagnostic.DoctorId = _doctorRepository.GetDoctorByUserId(_user.GetUserId().ToString()).Result.Id;
            await _diagnosticRepository.Create(AuditColumns<Diagnostic>(diagnostic, "Create", _user.GetUserId()));
        }

        public async Task Update(Diagnostic diagnostic)
        {
            if (!ExecuteValidation(new DiagnosticValidation(), diagnostic)) return;

            await _diagnosticRepository.Update(AuditColumns<Diagnostic>(diagnostic, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            await _diagnosticRepository.Delete(id);
        }

        public async Task Delete(Diagnostic diagnostic)
        {
            diagnostic.IsActive = diagnostic.IsActive.Value ? false : true;
            await _diagnosticRepository.Update(AuditColumns<Diagnostic>(diagnostic, "Update", _user.GetUserId()));
        }

        public void Dispose()
        {
            _diagnosticRepository?.Dispose();
            _doctorRepository?.Dispose();
        }
    }
}
