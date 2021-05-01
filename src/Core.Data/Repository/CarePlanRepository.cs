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
    public class CarePlanRepository : Repository<CarePlan>, ICarePlanRepository
    {
        private readonly IUser _user;

        public CarePlanRepository(MyDbContext context, IUser user) : base(context) { _user = user; }

        public async Task<IEnumerable<CarePlan>> GetAll(Guid patientId)
        {
            var doctorId = Db.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.CarePlans.AsNoTracking().Where(c => c.DoctorId == doctorId &&
                                                                c.PatientId == patientId)
                                                    .OrderByDescending(c => c.IsActive.Value).ThenByDescending(c => c.CreatedDate)
                                                    .ToListAsync();
        }

        public async Task<IEnumerable<CarePlan>> GetAllExceptDoctor(Guid patientId)
        {
            var doctorId = Db.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.CarePlans.AsNoTracking().Include(c => c.Doctor).Where(c => c.DoctorId != doctorId &&
                                                                                       c.PatientId == patientId)
                                                                           .OrderByDescending(c => c.IsActive.Value).ThenByDescending(c => c.CreatedDate)
                                                                           .ToListAsync();
        }

        public async Task<IEnumerable<CarePlan>> GetAllCarePlansByPacientId()
        {
            var patientId = Db.Patients.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.CarePlans.AsNoTracking().Include(c => c.Patient).Include(c => c.Doctor).Where(m => m.PatientId == patientId && m.IsActive.Value)
                                                                                                   .OrderBy(m => m.CreatedDate)
                                                                                                   .ToListAsync();
        }
    }
}
