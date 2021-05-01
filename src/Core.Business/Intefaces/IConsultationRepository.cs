using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IConsultationRepository : IRepository<Consultation>
    {
        Task<IEnumerable<Consultation>> GetAll(Guid patientId);

        Task<IEnumerable<Consultation>> GetAll(Guid patientId, Guid doctorId, Guid id);

        Task<IEnumerable<Consultation>> GetAllExceptDoctor(Guid patientId);

        Task<int> GetAllCountByDoctorId();
    }
}
