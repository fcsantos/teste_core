using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface INoticeService : IDisposable
    {
        Task Create(Notice note);
        Task Update(Notice note);
        Task Delete(Guid id);
        Task Delete(Notice notice);
    }
}