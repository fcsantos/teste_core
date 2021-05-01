using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<IEnumerable<Message>> GetAllMessagesSentByDoctorId();

        Task<IEnumerable<Message>> GetAllMessagesReplyByDoctorId();

        Task<IEnumerable<Message>> GetAllMessagesByPacientId();

        Task<IEnumerable<Message>> GetAllMessagesByPacientId(Guid id);

        Task<Message> GetByReplyMessageId(Guid id);

        Task<Message> GetByIdWithReplyMessage(Guid id);

        Task<IEnumerable<Message>> GetAllNewMessagesToPatientIsMailNotSender();

        Task<IEnumerable<Message>> GetAllNotReadMessagesToPatient();

        Task<IEnumerable<Message>> GetAllNotReadMessagesToDoctor();

        Task<IEnumerable<Message>> GetAllAwaitingResponseMessagesToPatient();
    }
}
