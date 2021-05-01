using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IServiceService : IDisposable
    {
        Task Create(Service action);
        Task Update(Service action);
        Task Delete(Guid id);
        Task Delete(Service service);
    } 
}