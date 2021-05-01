using System;
using System.Collections.Generic;

namespace Core.Business.Models
{
    public class Specialty : Entity
    {
        public Guid? ParentSpecialtyId { get; set; }
        public string Name { get; set; }
         
        /* EF Relation */
        public Specialty ParentSpecialty { get; set; }
        public IEnumerable<Specialty> SubSpecialties { get; set; }
        public IEnumerable<DoctorSpecialty> DoctorSpecialties{ get; set; }
    }
}
