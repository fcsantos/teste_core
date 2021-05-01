using System;

namespace Core.Business.Models
{
    public class NoticeUser : Entity
    {
        public Guid NoticeId { get; set; }
        public Notice Notice { get; set; }

        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
        public bool IsRead { get; set; }
    }
}
