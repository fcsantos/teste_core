using System;

namespace Core.Business.Models
{
    public class Consultation : Entity
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsActive { get; set; }

        /* EF Relation */
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}
