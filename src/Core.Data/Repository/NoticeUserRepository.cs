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
    public class NoticeUserRepository : Repository<NoticeUser>, INoticeUserRepository
    {
        private readonly IUser _user;

        public NoticeUserRepository(MyDbContext context, IUser user) : base(context) { _user = user; }

        public async Task<IEnumerable<NoticeUser>> GetByNoticeId(Guid noticeId)
        {
            var newGuid = new Guid();
            List<NoticeUser> r;
            if (newGuid.Equals(noticeId))
                r = await DbSet.Include(ds => ds.Patient)
                    .Where(s => s.Patient.IsActive == true)
                    .ToListAsync();
            else
                r = await DbSet.Include(ds => ds.Patient)
                    .Where(s => s.NoticeId == noticeId && s.Patient.IsActive == true)
                    .ToListAsync();
            return r;
        }
    }
}