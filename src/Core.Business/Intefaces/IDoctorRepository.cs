using Core.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Business.Intefaces
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<Doctor> GetWithSpecialityById(Guid id);
        Task<IEnumerable<Doctor>> GetAllWithSpeciality();
        Task<Doctor> GetDoctorWithSpecialityId(Guid id);
        Task<Doctor> GetDoctorByUserId(string userId);
    }
}
