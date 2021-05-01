using Core.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Business.Intefaces
{
    public interface IDoctorSpecialtyRepository : IRepository<DoctorSpecialty>
    {
        Task<IEnumerable<DoctorSpecialty>> GetByDoctorId(Guid doctorId);

        Task<bool> VerifySpecialtyHasDoctor(Guid specialtyId);
    }
}
