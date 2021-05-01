using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Web.Models
{
    public class ClinicalSummaryFacilitatorViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(5000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Patologia")]
        public Guid PathologyId { get; set; }

        [ScaffoldColumn(false)]
        public string PathologyName { get; set; }

        [ScaffoldColumn(false)]
        public IEnumerable<ClinicalSummaryFacilitatorViewModel> clinicalSummaryFacilitators { get; set; }

        public string TypeClinicalSummary { get; set; }

        public Guid DoctorId { get; set; }

        [ScaffoldColumn(false)]
        public string TypeClinicalSummaryFormat { get; set; }
    }
}
