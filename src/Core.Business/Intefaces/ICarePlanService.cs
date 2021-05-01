using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface ICarePlanService : IDisposable
    {
        Task Create(CarePlan carePlan);
        Task Update(CarePlan carePlan);
        Task Delete(Guid id);
        Task Delete(CarePlan carePlan);
    }
}
