using Core.Business.Models;
using System;
using System.Threading.Tasks;

namespace Core.Business.Intefaces
{
    public interface IDoctorService : IDisposable
    {
        Task Create(Doctor doctor);
        Task Update(Doctor doctor);
        Task Delete(Guid id);
        Task Delete(Doctor doctor);
        Task UpdateSpecialties(Doctor doctor);
    }
}
