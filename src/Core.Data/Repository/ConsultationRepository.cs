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
    public class ConsultationRepository : Repository<Consultation>, IConsultationRepository
    {
        private readonly IUser _user;

        public ConsultationRepository(MyDbContext context, IUser user) : base(context) { _user = user; }

        public async Task<IEnumerable<Consultation>> GetAll(Guid patientId)
        {
            var doctorId = Db.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.Consultations.AsNoTracking().Where(c => c.DoctorId == doctorId &&
                                                                    c.PatientId == patientId)
                                                        .OrderByDescending(c => c.Date).ThenByDescending(c => c.IsActive.Value)
                                                        .ToListAsync();
        }

        public async Task<IEnumerable<Consultation>> GetAll(Guid patientId, Guid doctorId, Guid id)
        {            
            return await Db.Consultations.AsNoTracking().Where(c => c.DoctorId == doctorId &&
                                                                    c.PatientId == patientId &&
                                                                    c.Id != id && c.IsActive.Value)
                                                        .ToListAsync();
        }

        public async Task<IEnumerable<Consultation>> GetAllExceptDoctor(Guid patientId)
        {
            var doctorId = Db.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.Consultations.AsNoTracking().Include(c => c.Doctor).Where(c => c.DoctorId != doctorId &&
                                                                                           c.PatientId == patientId)
                                                                               .OrderByDescending(c => c.Date).ThenByDescending(c => c.IsActive.Value)
                                                                               .ToListAsync();
        }

        public async Task<int> GetAllCountByDoctorId()
        {
            var doctorId = Db.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.Consultations.AsNoTracking().Where(p => p.DoctorId == doctorId).CountAsync();
        }
    }
}
