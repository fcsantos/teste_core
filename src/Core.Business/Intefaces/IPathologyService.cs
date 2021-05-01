using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IPathologyService : IDisposable
    {
        Task Create(Pathology pathology);
        Task Update(Pathology pathology);
        Task Delete(Guid id);
    }
}
