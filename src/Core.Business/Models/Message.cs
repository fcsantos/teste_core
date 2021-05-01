using System;

namespace Core.Business.Models
{
    public class Message : Entity
    {
        public Guid? ReplyMessageId { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public string Text { get; set; }
        public StatusMessage StatusMessage { get; set; }
        public bool? IsActive { get; set; }
        public bool IsReply { get; set; }
        public bool IsMailSender { get; set; }

        /* EF Relation */
        public Message ReplyMessage { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}
