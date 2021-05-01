using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Data.Repository
{
    public class DoctorSpecialtyRepository : Repository<DoctorSpecialty>, IDoctorSpecialtyRepository
    {
        public DoctorSpecialtyRepository(MyDbContext db) : base(db)
        {
        }

        public async Task<IEnumerable<DoctorSpecialty>> GetByDoctorId(Guid doctorId)
        {
            return await Db.DoctorSpecialties.AsNoTracking().Include(ds => ds.Specialty).Where(s => s.DoctorId == doctorId).ToListAsync();
        }

        public async Task<bool> VerifySpecialtyHasDoctor(Guid specialtyId)
        {
            return await Db.DoctorSpecialties.AsNoTracking().Where(ds => ds.SpecialtyId == specialtyId).AnyAsync();
        }
    }
}
