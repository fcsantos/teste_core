using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Models;
using Core.Business.Models.DTO;

namespace Core.Business.Intefaces
{
    public interface INoticeUserRepository : IRepository<NoticeUser>
    {
        Task<IEnumerable<NoticeUser>> GetByNoticeId(Guid id);
    }
  
}
