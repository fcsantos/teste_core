using System;
using System.Collections.Generic;

namespace Core.Business.Models
{
    public class Service : Entity
    {
        public string ServiceName { get; set; }
        public Guid DoctorId { get; set; }
        public bool? IsActive { get; set; }

        /* EF Relation */
        public ICollection<ServiceDoctor> ServiceDoctors { get; set; }
        public Doctor Doctor { get; set; }

    }
}
