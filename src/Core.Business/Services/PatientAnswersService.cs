using System;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class PatientAnswersService : BaseService, IPatientAnswersService
    {
        private readonly IPatientAnswersRepository _patientAnswersRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IUser _user;

        public PatientAnswersService(IPatientAnswersRepository patientAnswersRepository,
                                        IPatientRepository patientRepository,
        INotifier notifier, IUser user) : base(notifier)
        {
            _patientAnswersRepository = patientAnswersRepository;
            _patientRepository = patientRepository;
            _user = user;
        }

        public async Task Create(PatientAnswers patientAnswers)
        {
            if (!ExecuteValidation(new PatientAnswersValidation(), patientAnswers)) return;

            patientAnswers.PatientId = _patientRepository.GetPatientByUserId(_user.GetUserId().ToString()).Result.Id;
            await _patientAnswersRepository.Create(AuditColumns<PatientAnswers>(patientAnswers, "Create", _user.GetUserId()));
        }

        public async Task Update(PatientAnswers patientAnswers)
        {
            if (!ExecuteValidation(new PatientAnswersValidation(), patientAnswers)) return;
            await _patientAnswersRepository.Update(AuditColumns<PatientAnswers>(patientAnswers, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            await _patientAnswersRepository.Delete(id);
        }

        public async Task Delete(PatientAnswers patientAnswers)
        {
            //patientAnswers.IsActive = patientAnswers.IsActive.Value ? false : true;
            await _patientAnswersRepository.Update(AuditColumns<PatientAnswers>(patientAnswers, "Update", _user.GetUserId()));
        }

        public void Dispose()
        {
            _patientAnswersRepository?.Dispose();
            _patientRepository?.Dispose();
        }
    }
}
