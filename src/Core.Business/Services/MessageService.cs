using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class MessageService : BaseService, IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IUser _user;

        public MessageService(IMessageRepository messageRepository,
                              IDoctorRepository doctorRepository,
                              IPatientRepository patientRepository,
                              INotifier notifier, IUser user) : base(notifier)
        {
            _messageRepository = messageRepository;
            _user = user;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        public async Task Create(Message message)
        {
            if (!ExecuteValidation(new MessageValidation(), message)) return;

            if (_messageRepository.Search(m => m.Text == message.Text && 
                                               m.PatientId == message.PatientId && 
                                               m.IsActive == message.IsActive).Result.Any())
            {
                Notification("Já existe uma mensagem com este texto e que ainda está ativa, para o paciente selecionado.");
                return;
            }

            if (message.ReplyMessageId.HasValue)
            {
                message.StatusMessage = StatusMessage.Sent;
                message.IsReply = false;
                message.PatientId = _patientRepository.GetPatientByUserId(_user.GetUserId().ToString()).Result.Id;
            }
            else
            {
                message.StatusMessage = !message.IsReply ? StatusMessage.Sent : StatusMessage.AwaitingResponse;
                message.DoctorId = _doctorRepository.GetDoctorByUserId(_user.GetUserId().ToString()).Result.Id;
            }

            await _messageRepository.Create(AuditColumns<Message>(message, "Create", _user.GetUserId()));

            if (message.ReplyMessageId.HasValue)
            {
                message = await _messageRepository.GetById(message.ReplyMessageId.Value);
                message.IsReply = false;
                await Update(message);
            }
        }

        public async Task Update(Message message)
        {
            if (!ExecuteValidation(new MessageValidation(), message)) return;

            if (_messageRepository.Search(m => m.Text == message.Text && 
                                            m.PatientId == message.PatientId && 
                                            m.IsActive == message.IsActive && 
                                            m.Id != message.Id).Result.Any())
            {
                Notification("Já existe uma mensagem com este texto e que ainda está ativa, para o paciente selecionado.");
                return;
            }

            message.StatusMessage = message.StatusMessage == StatusMessage.AwaitingResponse ? StatusMessage.Answered : 
                                    message.StatusMessage == StatusMessage.Sent ? StatusMessage.Read : 
                                    message.StatusMessage == StatusMessage.Read ? StatusMessage.Sent : StatusMessage.Read;
            
            await _messageRepository.Update(AuditColumns<Message>(message, "Update", _user.GetUserId()));
        }

        public async Task UpdateEmailSender(Message message)
        {
            message.IsMailSender = true; 
            await _messageRepository.Update(AuditColumns<Message>(message, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            await _messageRepository.Delete(id);
        }

        public async Task Delete(Message message)
        {
            message.IsActive = message.IsActive.Value ? false : true;
            await _messageRepository.Update(AuditColumns<Message>(message, "Update", _user.GetUserId()));
        }

        public void Dispose()
        {
            _messageRepository?.Dispose();
            _doctorRepository?.Dispose();
            _patientRepository?.Dispose();
        }
    }
}
