using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface ISpecialtyService : IDisposable
    {
        Task Create(Specialty specialty);
        Task Update(Specialty specialty);
        Task Delete(Guid id); 
    }
}
