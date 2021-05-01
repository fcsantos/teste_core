using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IDiagnosticService : IDisposable
    {
        Task Create(Diagnostic diagnostic);
        Task Update(Diagnostic diagnostic);
        Task Delete(Guid id);
        Task Delete(Diagnostic diagnostic);
    }
}
