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
    public class PathologyRepository : Repository<Pathology>, IPathologyRepository
    {
        public PathologyRepository(MyDbContext context) : base(context){}


        public async Task<Pathology> GetSubPathologiesByPathologyId(Guid pathologyId)
        {
            return await Db.Pathologies.AsNoTracking().Include(s => s.SubPathologies)
                .FirstOrDefaultAsync(s => s.Id == pathologyId);
        }

        public async Task<IEnumerable<Pathology>> GetSubPathologiesPathologies()
        {
            return await Db.Pathologies.AsNoTracking().Include(s => s.ParentPathology).Include(s => s.SubPathologies).ToListAsync();
        }

        public async Task<IEnumerable<Pathology>> GetAllParentPathologies()
        {
            return await Db.Pathologies.AsNoTracking().Where(s => s.ParentPathology == null)
                .OrderBy(s => s.Name).ToListAsync();
        }

        public async Task<bool> VerifyPathologyHasParentPathology(Guid parentId)
        {
            return await Db.Pathologies.AsNoTracking().Where(s => s.ParentPathologyId == parentId).AnyAsync();
        }
    }
}
