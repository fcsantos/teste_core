using System.Collections.Generic;

namespace Core.Business.Models
{
    public class Inquiry : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }

        /* EF Relations */
        public ICollection<Question> Questions { get; set; }
        public ICollection<PatientAnswers> PatientAnswers { get; set; }
    }
}