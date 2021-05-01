using System;
using System.Collections.Generic;

namespace Core.Business.Models
{
    public class Question : Entity
    {
        public string Title { get; set; }
        public string Placeholder { get; set; }
        public bool? SingleLine { get; set; }
        public byte SortOrder { get; set; }
        public TypeOfAnswer TypeOfAnswer { get; set; }
        public Guid InquiryId { get; set; }


        /* EF Relations */
        public Inquiry Inquiry { get; set; }
        public ICollection<AnswerOption> AnswerOptions { get; set; }
    }
}

