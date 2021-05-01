using System;
using System.Collections.Generic;

namespace Core.Business.Models
{
    public class PatientAnswers : Entity
    {
        public Guid InquiryId { get; set; }
        public string InquiryTitle { get; set; }
        public Guid PatientId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid InquiryScheduleId { get; set; }
        public string QuestionTitle { get; set; }
        public Guid AnswerOptionId { get; set; }
        public string AnswerText { get; set; }
        public decimal AnswerValue { get; set; }

        /* EF Relations */


    }
}