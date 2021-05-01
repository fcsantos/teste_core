using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Business.Services
{
    public class PatientService : BaseService, IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IAddressRepository _enderecoRepository;
        private readonly IUser _user;

        public PatientService(IPatientRepository patienRepository,
                                IAddressRepository enderecoRepository,
                                INotifier notifier, IUser user) : base(notifier)
        {
            _patientRepository = patienRepository;
            _enderecoRepository = enderecoRepository;
            _user = user;
        }

        public async Task Create(Patient patient)
        {
            if (!ExecuteValidation(new PatientValidation(), patient)) return;

            if (_patientRepository.Search(p => p.Document == patient.Document && p.DocumentCard == patient.DocumentCard).Result.Any())
            {
                Notification("Já existe um paciente com estes documentos infomados.");
                return;
            }

            await _patientRepository.Create(AuditColumns<Patient, Address>(patient, "Create", _user.GetUserId(), patient.Address));
        }

        public async Task Update(Patient patient)
        {
            if (!ExecuteValidation(new PatientValidation(), patient)) return;

            if (_patientRepository.Search(p => p.Document == patient.Document && p.DocumentCard == patient.DocumentCard && p.Id != patient.Id).Result.Any())
            {
                Notification("Já existe um paciente com estes documentos infomados.");
                return;
            }

            await _patientRepository.Update(AuditColumns<Patient, Address>(patient, "Update", _user.GetUserId(), patient.Address));
        }

        public async Task UpdateEmailSender(Patient patient)
        {
            patient.IsMailSender = true;
            await _patientRepository.Update(AuditColumns<Patient>(patient, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            var patient = await _patientRepository.GetWithAddressById(id);

            if (patient != null)
            {
                await _patientRepository.Delete(id);
                if (patient.Address != null)
                {
                    await _enderecoRepository.Delete(patient.Address.Id);
                }
            }
        }

        public async Task Delete(Patient patient)
        {
            patient.IsActive = patient.IsActive.Value ? false : true;
            await _patientRepository.Update(AuditColumns<Patient>(patient, "Update", _user.GetUserId()));
        }        

        public void Dispose()
        {
            _patientRepository?.Dispose();
            _enderecoRepository?.Dispose();
        }
    }
}
