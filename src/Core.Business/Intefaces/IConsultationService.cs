using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IConsultationService : IDisposable
    {
        Task Create(Consultation consultation);
        Task Update(Consultation consultation);
        Task Delete(Guid id);
        Task Delete(Consultation consultation);
    }
}
