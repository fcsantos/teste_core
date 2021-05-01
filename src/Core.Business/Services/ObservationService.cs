using System;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class ObservationService : BaseService, IObservationService
    {
        private readonly IObservationRepository _observationRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUser _user;

        public ObservationService(IObservationRepository observationRepository,
                                  IDoctorRepository doctorRepository,
                                  INotifier notifier, IUser user) : base(notifier)
        {
            _observationRepository = observationRepository;
            _doctorRepository = doctorRepository;
            _user = user;
        }

        public async Task Create(Observation observation)
        {
            if (!ExecuteValidation(new ObservationValidation(), observation)) return;

            observation.DoctorId = _doctorRepository.GetDoctorByUserId(_user.GetUserId().ToString()).Result.Id;
            await _observationRepository.Create(AuditColumns<Observation>(observation, "Create", _user.GetUserId()));
        }

        public async Task Update(Observation observation)
        {
            if (!ExecuteValidation(new ObservationValidation(), observation)) return;

            await _observationRepository.Update(AuditColumns<Observation>(observation, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            await _observationRepository.Delete(id);
        }

        public async Task Delete(Observation observation)
        {
            observation.IsActive = observation.IsActive.Value ? false : true;
            await _observationRepository.Update(AuditColumns<Observation>(observation, "Update", _user.GetUserId()));
        }

        public void Dispose()
        {
            _observationRepository?.Dispose();
            _doctorRepository?.Dispose();
        }
    }
}
