using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Web.Models
{
    public class PatientAnswersViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid InquiryId { get; set; }
        public Guid InquiryScheduleId { get; set; }
        public string InquiryTitle { get; set; }
        public Guid PatientId { get; set; }
        public Guid QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public Guid AnswerOptionId { get; set; }
        public string AnswerText { get; set; }
        public decimal AnswerValue { get; set; }
    }
}