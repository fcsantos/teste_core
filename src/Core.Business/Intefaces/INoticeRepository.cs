using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Models;
using Core.Business.Models.DTO;

namespace Core.Business.Intefaces
{
    public interface INoticeRepository : IRepository<Notice>
    {
        Task<Notice> GetByNoticeId(Guid id);
        Task<IEnumerable<Notice>> GetAllNotice();
        Task<IEnumerable<NoticeUserDto>> GetAllCurrentNoticeBy();
    }
  
}
