using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class SpecialtyService : BaseService, ISpecialtyService
    {
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly IDoctorSpecialtyRepository _doctorSpecialtyRepository;
        private readonly IUser _user;
         
        public SpecialtyService(ISpecialtyRepository specialtyRepository,
                                IDoctorSpecialtyRepository doctorSpecialtyRepository,
                                INotifier notifier,
                                IUser user) : base(notifier)
        {
            _specialtyRepository = specialtyRepository;
            _doctorSpecialtyRepository = doctorSpecialtyRepository;
            _user = user;
        }

        public async Task Create(Specialty specialty)
        {
            if (!ExecuteValidation(new SpecialtyValidation(), specialty)) return;

            if (_specialtyRepository.Search(s => s.Name == specialty.Name).Result.Any())
            {
                Notification("Já existe uma especialidade com este nome infomado.");
                return;
            }

            await _specialtyRepository.Create(AuditColumns<Specialty>(specialty, "Create", _user.GetUserId()));
        }

        public async Task Update(Specialty specialty)
        {
            if (!ExecuteValidation(new SpecialtyValidation(), specialty)) return;

            if (_specialtyRepository.Search(s => s.Name == specialty.Name && s.Id != specialty.Id).Result.Any())
            {
                Notification("Já existe uma especialidade com este nome infomado.");
                return;
            }

            await _specialtyRepository.Update(AuditColumns<Specialty>(specialty, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            if (await _specialtyRepository.VerifySpecialtyHasParentSpecialty(id))
            {
                Notification("A especialidade possui sub especialidades cadastrados");
                return;
            }
            else if (await _doctorSpecialtyRepository.VerifySpecialtyHasDoctor(id))
            {
                Notification("A especialidade possui um ou mais médicos cadastrados");
                return;
            }
            await _specialtyRepository.Delete(id);
        }

        public void Dispose()
        {
            _specialtyRepository?.Dispose();
            _doctorSpecialtyRepository?.Dispose();
        }
    }
}
