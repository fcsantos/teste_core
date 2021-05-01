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
    public class SpecialtyRepository : Repository<Specialty>, ISpecialtyRepository
    {
        public SpecialtyRepository(MyDbContext context) : base(context) { }
         
        public async Task<Specialty> GetSubSpecialtiesBySpecialtyId(Guid specialtyId)
        {
            return await Db.Specialties.AsNoTracking().Include(s => s.SubSpecialties)
                .FirstOrDefaultAsync(s => s.Id == specialtyId);
        }

        public async Task<IEnumerable<Specialty>> GetSubSpecialtiesSpecialties()
        {
            return await Db.Specialties.AsNoTracking().Include(s => s.ParentSpecialty).Include(s => s.SubSpecialties).ToListAsync();
        }

        public async Task<IEnumerable<Specialty>> GetAllParentSpecialities()
        {
            return await Db.Specialties.AsNoTracking().Where(s => s.ParentSpecialtyId == null)
                .OrderBy(s => s.Name).ToListAsync();
        }

        public async Task<bool> VerifySpecialtyHasParentSpecialty(Guid parentId)
        {
            return await Db.Specialties.AsNoTracking().Where(s => s.ParentSpecialtyId == parentId).AnyAsync();
        }
    }
}
