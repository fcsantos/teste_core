using System;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repository
{
    public class AppControllerRepository : Repository<AppController>, IAppControllerRepository
    {
        public AppControllerRepository(MyDbContext context) : base(context)
        {
        }
        public async Task<AppController> GetControllerActions(Guid id)
        {
            return await Db.Controllers.AsNoTracking()
                .Include(c => c.Actions)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

    }
}