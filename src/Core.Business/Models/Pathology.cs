using System;
using System.Collections.Generic;

namespace Core.Business.Models
{
    public class Pathology : Entity
    {
        public Guid? ParentPathologyId { get; set; }
        public string Name { get; set; }

        /* EF Relation */
        public Pathology ParentPathology { get; set; }
        public IEnumerable<Pathology> SubPathologies { get; set; }
        public IEnumerable<ClinicalSummaryFacilitator> ClinicalSummaryFacilitators { get; set; }
    }
}
