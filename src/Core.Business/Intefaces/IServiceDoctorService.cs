using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IServiceDoctorService : IDisposable
    {
        Task Create(ServiceDoctor action);
        Task Update(ServiceDoctor action);
        Task Delete(Guid id);
    } 
}