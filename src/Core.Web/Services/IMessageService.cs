using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Web.Models;

namespace Core.Web.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageViewModel>> GetAllMessagesReplyByDoctorId();

        Task<IEnumerable<MessageViewModel>> GetAllMessagesSentByDoctorId();

        Task<IEnumerable<MessageViewModel>> GetAllMessagesByPacientId();

        Task<IEnumerable<MessageViewModel>> GetAllMessagesByPacientId(Guid id);

        Task<MessageViewModel> GetById(Guid id);

        Task<MessageViewModel> GetByReplyMessageId(Guid id);

        Task<MessageViewModel> GetByIdWithReplyMessage(Guid id);

        Task<IEnumerable<MessageViewModel>> GetAllNotReadMessagesToPatient();

        Task<IEnumerable<MessageViewModel>> GetAllNotReadMessagesToDoctor();

        Task<IEnumerable<MessageViewModel>> GetAllAwaitingResponseMessagesToPatient();

        Task<ResponseResult> Create(MessageViewModel messageViewModel);

        Task<ResponseResult> Update(MessageViewModel messageViewModel);

        Task<ResponseResult> Delete(Guid id);
    }
}
