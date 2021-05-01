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
    public class AppActionRepository : Repository<AppAction>, IAppActionRepository
    { 
        public AppActionRepository(MyDbContext context) : base(context) { }

        public async Task<IEnumerable<AppAction>> GetActionsByController(Guid Id) {
            return await Db.Actions.AsNoTracking().Where(a => a.ControllerId == Id).ToListAsync();
        }
    }
}