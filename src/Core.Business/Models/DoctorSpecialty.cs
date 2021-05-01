using System;

namespace Core.Business.Models
{
    public class DoctorSpecialty : Entity
    {
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public Guid SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }
    }
}
