using System;
using System.Collections.Generic;
using Core.Business.Models.Enumerators;

namespace Core.Business.Models
{
    public class Patient : Entity
    {
        public Guid AddressId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Document { get; set; }
        public string DocumentCard { get; set; }
        public string Email { get; set; }
        public string Cell { get; set; }
        public DateTime BirthDate { get; set; }
        public bool? IsActive { get; set; }
        public Gender Gender { get; set; }
        public bool IsMailSender { get; set; }

        /* EF Relations */
        public Address Address { get; set; }
        public IList<NoticeUser> NoticeUsers { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public IEnumerable<Consultation> Consultations { get; set; }
        public IEnumerable<Allergy> Allergies { get; set; }
        public IEnumerable<Diagnostic> Diagnostics { get; set; }
        public IEnumerable<CarePlan> CarePlans { get; set; }
       
    }
}
