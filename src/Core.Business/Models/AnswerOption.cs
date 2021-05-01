using System;
using System.Collections.Generic;

namespace Core.Business.Models
{
    public class AnswerOption : Entity
    {
        public Guid QuestionId { get; set; }
        public string Option { get; set; }
        public byte? SortOrder { get; set; }
        public decimal AnswerValue { get; set; }

        /* EF Relations */
        public Question Question { get; set; }
    }
}