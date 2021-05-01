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
    public class DiagnosticRepository : Repository<Diagnostic>, IDiagnosticRepository
    {
        private readonly IUser _user;

        public DiagnosticRepository(MyDbContext context, IUser user) : base(context) { _user = user; }

        public async Task<IEnumerable<Diagnostic>> GetAll(Guid patientId)
        {
            var doctorId = Db.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.Diagnostics.AsNoTracking().Where(c => c.DoctorId == doctorId &&
                                                                  c.PatientId == patientId)
                                                      .OrderByDescending(c => c.IsActive.Value).ThenByDescending(c => c.CreatedDate)
                                                      .ToListAsync();
        }

        public async Task<IEnumerable<Diagnostic>> GetAllExceptDoctor(Guid patientId)
        {
            var doctorId = Db.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.Diagnostics.AsNoTracking().Include(c => c.Doctor).Where(c => c.DoctorId != doctorId &&
                                                                                         c.PatientId == patientId)
                                                                             .OrderByDescending(c => c.IsActive.Value).ThenByDescending(c => c.CreatedDate)
                                                                             .ToListAsync();
        }

        public async Task<int> GetAllCountByDoctorId()
        {
            var doctorId = Db.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.Diagnostics.AsNoTracking().Where(p => p.DoctorId == doctorId && p.IsActive.Value).CountAsync();
        }
    }
}
