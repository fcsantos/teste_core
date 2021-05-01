using System;
using System.Collections.Generic;

namespace Core.Business.Models
{
    public class Doctor : Entity
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Cell { get; set; }
        public string DocumentCard { get; set; }
        public bool? IsActive { get; set; }

        /* EF Relations */
        public ICollection<ServiceDoctor> ServiceDoctors { get; set; }
        public IEnumerable<Service> Services { get; set; }
        public ICollection<DoctorSpecialty> DoctorSpecialties { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public IEnumerable<Consultation> Consultations { get; set; }
        public IEnumerable<Allergy> Allergies { get; set; }
        public IEnumerable<Diagnostic> Diagnostics { get; set; }
        public IEnumerable<CarePlan> CarePlans { get; set; }
        public IEnumerable<ClinicalSummaryFacilitator> ClinicalSummaryFacilitators { get; set; }
    }
}