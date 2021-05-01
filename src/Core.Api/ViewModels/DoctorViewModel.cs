using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Api.ViewModels
{
    public class DoctorViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        public string[] SpecialtyGuids { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public ICollection<SpecialtyViewModel> Specialties { get; set; }

        public IEnumerable<SpecialtyViewModel> SystemSpecialties { get; set; }

        [ScaffoldColumn(false)]
        public string SpecialtiesNames { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "E-Mail em formato inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Phone(ErrorMessage = "Telemovel em formato inválido.")]
        public string Cell { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(9, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 9)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Deve ser numérico")]
        public string DocumentCard { get; set; }

        public bool? IsActive { get; set; }

        public string UserId { get; set; }

        [ScaffoldColumn(false)]
        public string Ativo { get; set; }
    }
}
