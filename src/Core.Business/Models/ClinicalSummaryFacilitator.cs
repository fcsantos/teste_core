using System;

namespace Core.Business.Models
{
    public class ClinicalSummaryFacilitator : Entity
    {
        public Guid PathologyId { get; set; }
        public Guid DoctorId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public TypeClinicalSummary TypeClinicalSummary { get; set; }

        /* EF Relations */
        public Pathology Pathology { get; set; }
        public Doctor Doctor { get; set; }
    }
}
