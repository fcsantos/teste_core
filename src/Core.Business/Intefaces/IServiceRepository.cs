using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IServiceRepository : IRepository<Service>
    {

        Task<Service> GetServiceDoctorsByServiceId(Guid serviceId);
        Task<IEnumerable<Service>> GetAllServices();

    }
} 
