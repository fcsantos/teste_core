using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Web.Models
{
    public class ObservationViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(5000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        public bool? IsActive { get; set; }

        [ScaffoldColumn(false)]
        public string DateFormat { get; set; }

        [ScaffoldColumn(false)]
        public string DoctorName { get; set; }

        [ScaffoldColumn(false)]
        public string PatientName { get; set; }

        [ScaffoldColumn(false)]
        public string Ativo { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Facilitador de preenchimento")]
        public string ClinicalSummaryFacilitator { get; set; }


        public PatientViewModel Patient { get; set; }
        public DoctorViewModel Doctor { get; set; }
    }
}
