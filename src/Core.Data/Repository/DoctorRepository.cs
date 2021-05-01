using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repository
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(MyDbContext context) : base(context)
        {
        }

        public async Task<Doctor> GetWithSpecialityById(Guid id)
        {
            return await Db.Doctors.AsNoTracking().Include(de => de.DoctorSpecialties).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Doctor>> GetAllWithSpeciality()
        {
            return await Db.Doctors.AsNoTracking().Include(de => de.DoctorSpecialties)
                .ThenInclude(s => s.Specialty).ToListAsync();
        }

        public async Task<Doctor> GetDoctorWithSpecialityId(Guid id)
        {
            return await Db.Doctors.AsNoTracking().Include(de => de.DoctorSpecialties)
                .ThenInclude(s => s.Specialty)
                .FirstOrDefaultAsync(doc => doc.Id == id);
        }

        public async Task<Doctor> GetDoctorByUserId(string userId)
        {
            return await Db.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == userId);
        }
    }
}
