using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IAllergyRepository : IRepository<Allergy>
    {
        Task<IEnumerable<Allergy>> GetAll(Guid patientId);
        Task<IEnumerable<Allergy>> GetAllExceptDoctor(Guid patientId);
    }
}
