using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IObservationRepository : IRepository<Observation>
    {
        Task<IEnumerable<Observation>> GetAll(Guid patientId);
        Task<IEnumerable<Observation>> GetAllExceptDoctor(Guid patientId);
    }
}
