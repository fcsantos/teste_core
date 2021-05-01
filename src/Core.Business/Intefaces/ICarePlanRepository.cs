using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface ICarePlanRepository : IRepository<CarePlan>
    {
        Task<IEnumerable<CarePlan>> GetAll(Guid patientId);
        Task<IEnumerable<CarePlan>> GetAllExceptDoctor(Guid patientId);
        Task<IEnumerable<CarePlan>> GetAllCarePlansByPacientId();
    }
}
