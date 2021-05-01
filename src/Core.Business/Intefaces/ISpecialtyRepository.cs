using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface ISpecialtyRepository : IRepository<Specialty>
    {
        Task<IEnumerable<Specialty>> GetSubSpecialtiesSpecialties();
        Task<Specialty> GetSubSpecialtiesBySpecialtyId(Guid specialtyId);
        Task<IEnumerable<Specialty>> GetAllParentSpecialities(); 
        Task<bool> VerifySpecialtyHasParentSpecialty(Guid parentId);
    }
}
