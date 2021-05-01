using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class ServiceService : BaseService, IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceDoctorRepository _serviceDoctorRepository;
        private readonly IUser _user;
        public ServiceService(IServiceRepository serviceRepository,
            IServiceDoctorRepository serviceDoctorRepository,
                                 INotifier notificador,
                                 IUser user) : base(notificador)
        {
            _serviceRepository = serviceRepository;
            _serviceDoctorRepository = serviceDoctorRepository;
            _user = user;

        }

        public async Task Create(Service service)
        {
            if (!ExecuteValidation(new ServiceValidation(), service)) return;

            if (_serviceRepository.Search(s => s.ServiceName == service.ServiceName).Result.Any())
            {
                Notification("Já existe um serviço com este nome infomado.");
                return;
            }
            await _serviceRepository.Create(AuditColumns<Service>(service, "Create", _user.GetUserId()));

        }

        public async Task Update(Service service)
        {
            if (!ExecuteValidation(new ServiceValidation(), service)) return;

            await UpdateServiceDoctors(service);
            service.IsActive = true;
            await _serviceRepository.Update(AuditColumns<Service>(service, "Update", _user.GetUserId()));
        }

        public async Task UpdateServiceDoctors(Service service)
        {
            var savedServiceDoctors = _serviceDoctorRepository.GetByServiceId(service.Id).Result;

            foreach (var serviceDoctor in savedServiceDoctors)
                await _serviceDoctorRepository.Delete(serviceDoctor);
        }

        public async Task Delete(Service service)
        {
            service.IsActive = service.IsActive.Value ? false : true;
            await _serviceRepository.Update(AuditColumns<Service>(service, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            await _serviceRepository.Delete(id);
        }

        public void Dispose()
        {
            _serviceRepository?.Dispose();
            _serviceDoctorRepository?.Dispose();
        }


    }
}