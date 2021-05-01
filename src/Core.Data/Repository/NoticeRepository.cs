using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.DTO;
using Core.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repository
{
    public class NoticeRepository : Repository<Notice>, INoticeRepository
    {
        private readonly IUser _user;

        public NoticeRepository(MyDbContext context, IUser user) : base(context) { _user = user; }

        public async Task<Notice> GetByNoticeId(Guid noticeId)
        {
            return await Db.Notices.AsNoTracking().Include(i => i.NoticeUsers).Where(s => s.Id == noticeId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Notice>> GetAllNotice()
        {
            return await Db.Notices.AsNoTracking().Include(n => n.NoticeUsers).ThenInclude(n => n.Patient).ToListAsync();
        }

        public async Task<IEnumerable<NoticeUserDto>> GetAllCurrentNoticeBy()
        {
            var patientId = Db.Patients.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;

            var notices = await Db.Notices.ToListAsync();
            var resultNotices = notices.Where(x => Convert.ToDateTime(x.StartDate.ToShortDateString()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString()) &&
                                                                                        Convert.ToDateTime(x.EndDate.ToShortDateString()).AddDays(1).AddTicks(-3) >= Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddDays(1).AddTicks(-3) &&
                                                                                        x.SendToAllUsers).ToList();
            var dto1 = from n in resultNotices
                       select new NoticeUserDto
                       {
                           Id = n.Id,
                           Description = n.Description,
                           Status = n.Status.ToString(),
                           SendToAllUsers = n.SendToAllUsers,
                           StartDate = n.StartDate,
                           EndDate = n.EndDate
                       };

            var noticesUser = await Db.NoticeUsers.AsNoTracking().Include(x => x.Notice).ToListAsync();
            var resultNoticesUser = noticesUser.Where(x => Convert.ToDateTime(x.Notice.StartDate.ToShortDateString()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString()) &&
                                                                                                 Convert.ToDateTime(x.Notice.EndDate.ToShortDateString()).AddDays(1).AddTicks(-3) >= Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddDays(1).AddTicks(-3) &&
                                                                                                 x.PatientId == patientId).ToList();


            var dto2 = from n in resultNoticesUser
                       select new NoticeUserDto
                       {
                           Id = n.NoticeId,
                           Description = n.Notice.Description,
                           Status = n.Notice.Status.ToString(),                           
                           StartDate = n.Notice.StartDate,
                           EndDate = n.Notice.EndDate
                       };

            return dto1.Union(dto2);
        }
    }
}