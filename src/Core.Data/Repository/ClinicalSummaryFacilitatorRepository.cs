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
    public class ClinicalSummaryFacilitatorRepository : Repository<ClinicalSummaryFacilitator>, IClinicalSummaryFacilitatorRepository
    {
        private readonly IUser _user;

        public ClinicalSummaryFacilitatorRepository(MyDbContext context, IUser user) : base(context) { _user = user; }

        public async Task<bool> VerifyClinicalSummaryFacilitatorHasPathology(Guid pathologyId)
        {
            return await Db.ClinicalSummaryFacilitators.AsNoTracking().Where(c => c.PathologyId == pathologyId).AnyAsync();
        }

        public async Task<IEnumerable<ClinicalSummaryFacilitator>> GetAllByPathologyId(Guid pathologyId)
        {
            var doctorId = Db.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.ClinicalSummaryFacilitators.AsNoTracking().Include(c => c.Pathology).Where(c => c.PathologyId == pathologyId && c.DoctorId == doctorId).ToListAsync();
        }
    }
}
