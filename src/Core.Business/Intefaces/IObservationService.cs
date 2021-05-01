using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IObservationService : IDisposable
    {
        Task Create(Observation observation);
        Task Update(Observation observation);
        Task Delete(Guid id);
        Task Delete(Observation observation);
    }
}
