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
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private readonly IUser _user;
        public PatientRepository(MyDbContext context, IUser user) : base(context) { _user = user; }

        public async Task<Patient> GetWithAddressById(Guid id)
        {
            return await Db.Patients.AsNoTracking()
                                    .Include(p => p.Address)
                                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Patient> GetPatientByUserId(string userId)
        {
            return await Db.Patients.AsNoTracking()
                                    .Include(p => p.Address)
                                    .Where(p => p.UserId.Equals(userId)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Patient>> GetPatientIsMailNotSender()
        {
            return await Db.Patients.AsNoTracking().Where(p => !p.IsMailSender).ToListAsync();
        }

        public async Task<int> GetAllActiveCount()
        {
            return await Db.Patients.AsNoTracking().Where(p => p.IsActive.Value).CountAsync();
        }
    }
}
