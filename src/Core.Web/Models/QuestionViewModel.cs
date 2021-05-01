using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Web.Models
{
    public class QuestionViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Placeholder { get; set; }
        public bool? SingleLine { get; set; }
        public byte? SortOrder { get; set; }
        public string TypeOfAnswer { get; set; }
        public Guid InquiryId { get; set; }
        public ICollection<AnswerOptionsViewModel> AnswerOptions { get; set; }
    }

    public class AnswerOptionsViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Option { get; set; }
        public byte? SortOrder { get; set; }
        public decimal AnswerValue { get; set; }
    }

}