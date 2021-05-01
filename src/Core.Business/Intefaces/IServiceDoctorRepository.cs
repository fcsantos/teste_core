using Core.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Business.Intefaces
{
    public interface IServiceDoctorRepository : IRepository<ServiceDoctor>
    {
        Task<IEnumerable<ServiceDoctor>> GetByServiceId(Guid serviceId);
    }
}
