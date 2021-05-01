using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IMessageService : IDisposable
    {
        Task Create(Message message);
        Task Update(Message message);
        Task UpdateEmailSender(Message message);
        Task Delete(Guid id);
        Task Delete(Message message);
    }
}
