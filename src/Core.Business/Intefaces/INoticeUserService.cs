using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface INoticeUserService : IDisposable
    {
        Task Create(NoticeUser note);
        Task Update(NoticeUser note);
        Task Delete(Guid id);
        Task Delete(NoticeUser noticeUser);
    }
}