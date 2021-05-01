using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Business.Services
{
    public class DoctorService : BaseService, IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDoctorSpecialtyRepository _doctorSpecialtyRepository;
        private readonly IUser _user;

        public DoctorService(IDoctorRepository doctorRepository,
                                IDoctorSpecialtyRepository doctorSpecialtyRepository,
                                INotifier notificador, IUser user) : base(notificador)
        {
            _doctorRepository = doctorRepository;
            _doctorSpecialtyRepository = doctorSpecialtyRepository;
            _user = user;
        }
        public async Task Create(Doctor doctor)
        {
            if (!ExecuteValidation(new DoctorValidation(), doctor)) return;

            if (_doctorRepository.Search(d => d.DocumentCard == doctor.DocumentCard).Result.Any())
            {
                Notification("Já existe um médico com estes documentos infomados.");
                return;
            }
            else if (doctor.DoctorSpecialties.Count() == 0)
            {
                Notification("Favor selecione uma ou mais especialidades ao médico.");
                return;
            }

            await _doctorRepository.Create(AuditColumns<Doctor>(doctor, "Create", _user.GetUserId()));
        }

        public async Task Update(Doctor doctor)
        {
            if (!ExecuteValidation(new DoctorValidation(), doctor)) return;

            if (_doctorRepository.Search(p => p.DocumentCard == doctor.DocumentCard && p.Id != doctor.Id).Result.Any())
            {
                Notification("Já existe um médico com estes documentos infomados.");
                return;
            }
            else if (doctor.DoctorSpecialties.Count() == 0)
            {
                Notification("Favor selecione uma ou mais especialidades ao médico.");
                return;
            }

            await UpdateSpecialties(doctor);

            await _doctorRepository.Update(AuditColumns<Doctor>(doctor, "Update", _user.GetUserId()));
        }

        public async Task UpdateSpecialties(Doctor doctor)
        {
            var savedDoctorSpecialties = await _doctorSpecialtyRepository.GetByDoctorId(doctor.Id);

            foreach (var doctorSpecialty in savedDoctorSpecialties)
                await _doctorSpecialtyRepository.Delete(doctorSpecialty);
        }

        public async Task Delete(Guid id)
        {
            var doctor = await _doctorRepository.GetDoctorWithSpecialityId(id);

            if (doctor != null)
            {
                if (doctor.DoctorSpecialties != null)
                {
                    foreach (var doctorSpecialty in doctor.DoctorSpecialties.ToArray())
                    {
                        await _doctorSpecialtyRepository.Delete(doctorSpecialty);
                    }
                }

                await _doctorRepository.Delete(doctor);
            }
        }

        public async Task Delete(Doctor doctor)
        {
            doctor.IsActive = doctor.IsActive.Value ? false : true;
            await _doctorRepository.Update(AuditColumns<Doctor>(doctor, "Update", _user.GetUserId()));
        }

        public void Dispose()
        {
            _doctorRepository?.Dispose();
            _doctorSpecialtyRepository?.Dispose();
        }
    }
}
