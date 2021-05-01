using System;

namespace Core.Business.Models
{
    public class InquirySchedule : Entity
    {
        public Guid InquiryId { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }

        public bool? Answered { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsMailSender { get; set; }

        /* EF Relations */
        public Inquiry Inquiry { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}