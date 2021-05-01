using System;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class AllergyService : BaseService, IAllergyService
    {
        private readonly IAllergyRepository _allergyRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUser _user;

        public AllergyService(IAllergyRepository allergyRepository,
                              IDoctorRepository doctorRepository,
                              INotifier notifier, IUser user) : base(notifier)
        {
            _allergyRepository = allergyRepository;
            _doctorRepository = doctorRepository;
            _user = user;
        }

        public async Task Create(Allergy allergy)
        {
            if (!ExecuteValidation(new AllergyValidation(), allergy)) return;

            allergy.DoctorId = _doctorRepository.GetDoctorByUserId(_user.GetUserId().ToString()).Result.Id;
            await _allergyRepository.Create(AuditColumns<Allergy>(allergy, "Create", _user.GetUserId()));
        }

        public async Task Update(Allergy allergy)
        {
            if (!ExecuteValidation(new AllergyValidation(), allergy)) return;

            await _allergyRepository.Update(AuditColumns<Allergy>(allergy, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            await _allergyRepository.Delete(id);
        }

        public async Task Delete(Allergy allergy)
        {
            allergy.IsActive = allergy.IsActive.Value ? false : true;
            await _allergyRepository.Update(AuditColumns<Allergy>(allergy, "Update", _user.GetUserId()));
        }

        public void Dispose()
        {
            _allergyRepository?.Dispose();
            _doctorRepository?.Dispose();
        }
    }
}
