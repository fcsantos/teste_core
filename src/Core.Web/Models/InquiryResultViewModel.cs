using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Web.Models
{
    public class InquiryResultViewModel
    {
        public Guid InquiryScheduleId { get; set; }
        public string InquiryTitle { get; set; }
        public string? InquiryDescription { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public decimal AnswerValueSum { get; set; }
        public DateTime UpdatedDate { get; set; }
        public IEnumerable<PatientAnswersViewModel> PatientAnswers { get; set; }

    }
}