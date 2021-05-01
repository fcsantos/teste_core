using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Web.Models
{
    public class DoctorViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Data de Nascimento"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        public string[] SpecialtyGuids { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Especialidades")]
        public ICollection<SpecialtyViewModel> Specialties { get; set; }

        [ScaffoldColumn(false)]
        public IEnumerable<SpecialtyViewModel> SystemSpecialties { get; set; }

        [Display(Name = "Nome de Especialidades")]
        public string SpecialtiesNames { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "E-Mail em formato inválido.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Phone(ErrorMessage = "Telemovel em formato inválido.")]
        [Display(Name = "Telemóvel")]
        public string Cell { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(9, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 9)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Deve ser numérico")]
        [Display(Name = "Número de identificação")]
        public string DocumentCard { get; set; }

        public bool? IsActive { get; set; }

        public string UserId { get; set; }

        [ScaffoldColumn(false)]
        public string Ativo { get; set; }

        public DoctorViewModel()
        {
            Specialties = new List<SpecialtyViewModel>();
        }
    }
}
