using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IDiagnosticRepository : IRepository<Diagnostic>
    {
        Task<IEnumerable<Diagnostic>> GetAll(Guid patientId);
        Task<IEnumerable<Diagnostic>> GetAllExceptDoctor(Guid patientId);
        Task<int> GetAllCountByDoctorId();
    }
}
