using System;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class CarePlanService : BaseService, ICarePlanService
    {
        private readonly ICarePlanRepository _carePlanRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUser _user;

        public CarePlanService(ICarePlanRepository carePlanRepository,
                               IDoctorRepository doctorRepository,
                               INotifier notifier, IUser user) : base(notifier)
        {
            _carePlanRepository = carePlanRepository;
            _doctorRepository = doctorRepository;
            _user = user;
        }

        public async Task Create(CarePlan carePlan)
        {
            if (!ExecuteValidation(new CarePlanValidation(), carePlan)) return;

            carePlan.DoctorId = _doctorRepository.GetDoctorByUserId(_user.GetUserId().ToString()).Result.Id;
            await _carePlanRepository.Create(AuditColumns<CarePlan>(carePlan, "Create", _user.GetUserId()));
        }

        public async Task Update(CarePlan carePlan)
        {
            if (!ExecuteValidation(new CarePlanValidation(), carePlan)) return;

            await _carePlanRepository.Update(AuditColumns<CarePlan>(carePlan, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            await _carePlanRepository.Delete(id);
        }

        public async Task Delete(CarePlan carePlan)
        {
            carePlan.IsActive = carePlan.IsActive.Value ? false : true;
            await _carePlanRepository.Update(AuditColumns<CarePlan>(carePlan, "Update", _user.GetUserId()));
        }

        public void Dispose()
        {
            _carePlanRepository?.Dispose();
            _doctorRepository?.Dispose();
        }
    }
}
