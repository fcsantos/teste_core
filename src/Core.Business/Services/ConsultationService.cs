using System;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class ConsultationService : BaseService, IConsultationService
    {
        private readonly IConsultationRepository _consultationRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUser _user;

        public ConsultationService(IConsultationRepository consultationRepository,
                                   IDoctorRepository doctorRepository,
                                   INotifier notifier, 
                                   IUser user) : base(notifier)
        {
            _consultationRepository = consultationRepository;
            _doctorRepository = doctorRepository;
            _user = user;
        }

        public async Task Create(Consultation consultation)
        {
            if (!ExecuteValidation(new ConsultationValidation(), consultation)) return;

            consultation.DoctorId = _doctorRepository.GetDoctorByUserId(_user.GetUserId().ToString()).Result.Id;
            await _consultationRepository.Create(AuditColumns<Consultation>(consultation, "Create", _user.GetUserId()));

            var consultations = await _consultationRepository.GetAll(consultation.PatientId, consultation.DoctorId, consultation.Id);
            foreach (var c in consultations)
            {
                c.IsActive = false;
                await _consultationRepository.Update(AuditColumns<Consultation>(c, "Update", _user.GetUserId()));
            }
        }

        public async Task Update(Consultation consultation)
        {
            if (!ExecuteValidation(new ConsultationValidation(), consultation)) return;

            await _consultationRepository.Update(AuditColumns<Consultation>(consultation, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            await _consultationRepository.Delete(id);
        }

        public async Task Delete(Consultation consultation)
        {
            consultation.IsActive = consultation.IsActive.Value ? false : true;
            await _consultationRepository.Update(AuditColumns<Consultation>(consultation, "Update", _user.GetUserId()));
        }

        public void Dispose()
        {
            _consultationRepository?.Dispose();
            _doctorRepository?.Dispose();
        }
    }
}
