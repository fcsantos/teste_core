using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Api.ViewModels
{
    public class ClinicalSummaryFacilitatorViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(5000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Description { get; set; }

        public Guid PathologyId { get; set; }

        [ScaffoldColumn(false)]
        public string PathologyName { get; set; }

        public string TypeClinicalSummary { get; set; }

        public Guid DoctorId { get; set; }

        [ScaffoldColumn(false)]
        public string TypeClinicalSummaryFormat { get; set; }
    }
}
