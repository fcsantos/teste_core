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
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        private readonly IUser _user;

        public MessageRepository(MyDbContext context, IUser user) : base(context) { _user = user; }

        public async Task<IEnumerable<Message>> GetAllMessagesReplyByDoctorId()
        {
            var doctorId = Db.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.Messages.AsNoTracking().Include(m => m.Patient).Where(m => m.DoctorId == doctorId &&
                                                                        m.ReplyMessageId.HasValue)
                                                            .OrderBy(m => m.StatusMessage)
                                                            .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetAllMessagesSentByDoctorId()
        {
            var doctorId = Db.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.Messages.AsNoTracking().Include(m => m.Patient).Where(m => m.DoctorId == doctorId &&
                                                                                       !m.ReplyMessageId.HasValue)
                                                            .OrderBy(m => m.StatusMessage)
                                                            .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetAllMessagesByPacientId()
        {
            var patientId = Db.Patients.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.Messages.AsNoTracking().Include(m => m.Patient).Include(d => d.Doctor).Include(r => r.ReplyMessage)
                                                            .Where(m => m.PatientId == patientId && !m.ReplyMessageId.HasValue)
                                                            .OrderBy(m => m.StatusMessage).ThenByDescending(m => m.CreatedDate)
                                                            .ToListAsync();   

        }

        public async Task<IEnumerable<Message>> GetAllMessagesByPacientId(Guid id)
        {
            return await Db.Messages.AsNoTracking().Include(m => m.Patient).Include(d => d.Doctor).Include(r => r.ReplyMessage)
                                                            .Where(m => m.PatientId == id && !m.ReplyMessageId.HasValue)
                                                            .OrderBy(m => m.StatusMessage).ThenByDescending(m => m.CreatedDate)
                                                            .ToListAsync();

        }

        public async Task<Message> GetByIdWithReplyMessage(Guid id)
        {
            return await Db.Messages.AsNoTracking().Include(d => d.Doctor).Where(o => o.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Message> GetByReplyMessageId(Guid id)
        {
            return await Db.Messages.AsNoTracking().Include(p => p.Patient).Include(r => r.ReplyMessage).Where(o => o.ReplyMessageId == id).FirstOrDefaultAsync(); ;
        }

        public async Task<IEnumerable<Message>> GetAllNewMessagesToPatientIsMailNotSender()
        {
            return await Db.Messages.AsNoTracking().Include(m => m.Patient).Include(m => m.Doctor).Where(m => !m.IsMailSender && m.StatusMessage == StatusMessage.AwaitingResponse && m.IsReply).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetAllNotReadMessagesToPatient()
        {
            var patientId = Db.Patients.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.Messages.AsNoTracking().Where(m => m.StatusMessage == StatusMessage.Sent && 
                                                              !m.IsReply && 
                                                              !m.ReplyMessageId.HasValue &&
                                                               m.PatientId == patientId).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetAllNotReadMessagesToDoctor()
        {
            var doctorId = Db.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.Messages.AsNoTracking().Include(m => m.Patient).Where(m => m.DoctorId == doctorId &&
                                                               m.ReplyMessageId.HasValue &&
                                                               m.StatusMessage == StatusMessage.Sent).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetAllAwaitingResponseMessagesToPatient()
        {
            var patientId = Db.Patients.AsNoTracking().FirstOrDefaultAsync(d => d.UserId == _user.GetUserId().ToString()).Result.Id;
            return await Db.Messages.AsNoTracking().Where(m => m.StatusMessage == StatusMessage.AwaitingResponse &&
                                                               m.IsReply &&
                                                              !m.ReplyMessageId.HasValue &&
                                                               m.PatientId == patientId).ToListAsync();
        }
    }
}
