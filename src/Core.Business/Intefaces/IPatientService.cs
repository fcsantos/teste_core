using Core.Business.Models;
using System;
using System.Threading.Tasks;

namespace Core.Business.Intefaces
{
    public interface IPatientService : IDisposable
    {
        Task Create(Patient pacient);
        Task Update(Patient pacient);
        Task UpdateEmailSender(Patient patient);
        Task Delete(Guid id);
        Task Delete(Patient patient);
    }
}
