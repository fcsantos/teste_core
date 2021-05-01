using System;

namespace Core.Business.Models
{
    public class ServiceDoctor : Entity
    {
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
