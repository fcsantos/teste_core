using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IAllergyService : IDisposable
    {
        Task Create(Allergy allergy);
        Task Update(Allergy allergy);
        Task Delete(Guid id);
        Task Delete(Allergy allergy);
    }
}
