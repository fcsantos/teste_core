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
    public class ServiceDoctorRepository : Repository<ServiceDoctor>, IServiceDoctorRepository
    {
        public ServiceDoctorRepository(MyDbContext db) : base(db)
        {
        }

        public async Task<IEnumerable<ServiceDoctor>> GetByServiceId(Guid serviceId)
        {
            var newGuid = new Guid();
            List<ServiceDoctor> r;
            if (newGuid.Equals(serviceId))
                r = await DbSet.Include(ds => ds.Doctor)
                    .Where(s => s.Doctor.IsActive == true)
                    .ToListAsync();
            else
                r = await DbSet.Include(ds => ds.Doctor)
                    .Where(s => s.ServiceId == serviceId && s.Doctor.IsActive == true)
                    .ToListAsync();
            return r;
        }

    }
}
