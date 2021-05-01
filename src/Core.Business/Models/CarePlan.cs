using System;

namespace Core.Business.Models
{
    public class CarePlan : Entity
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }

        /* EF Relation */
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}
