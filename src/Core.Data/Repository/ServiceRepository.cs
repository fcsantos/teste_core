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
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(MyDbContext context) : base(context)
        {
        }
        public async Task<Service> GetServiceDoctorsByServiceId(Guid serviceId)
        {
            var _ds = await Db.Services.Include(i => i.ServiceDoctors)
                .Include(d => d.Doctor)
                .Where(s => s.Doctor.IsActive.Equals(true))
                .FirstOrDefaultAsync(s => s.Id == serviceId);
            return _ds;
        }
        public async Task<IEnumerable<Service>> GetAllServices()
        {
            var _as = await Db.Services.Include(i => i.ServiceDoctors)
                .Include(d => d.Doctor)
                .Where(s => s.Doctor.IsActive.Equals(true))
                .ToListAsync();
            return _as;
        }

    }
}