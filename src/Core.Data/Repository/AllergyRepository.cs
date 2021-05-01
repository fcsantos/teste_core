using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repository
{
    public class AllergyRepository : Repository<Allergy>, IAllergyRepository
    {
        private readonly IUser _user;

        public AllergyRepository(MyDbContext context, IUser user) : base(context) { _user = user; }

        public async Task<IEnumerable<Allergy>> GetAll(Guid patientId)
        {
            var doctorId = Db.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.Allergies.AsNoTracking().Where(c => c.DoctorId == doctorId &&
                                                                    c.PatientId == patientId)
                                                    .OrderByDescending(c => c.IsActive.Value).ThenByDescending(c => c.CreatedDate)
                                                    .ToListAsync();
        }

        public async Task<IEnumerable<Allergy>> GetAllExceptDoctor(Guid patientId)
        {
            var doctorId = Db.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.Allergies.AsNoTracking().Include(c => c.Doctor).Where(c => c.DoctorId != doctorId &&
                                                                                       c.PatientId == patientId)
                                                                           .OrderByDescending(c => c.IsActive.Value).ThenByDescending(c => c.CreatedDate)
                                                                           .ToListAsync();
        }
    }
}
