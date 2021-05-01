using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Web.Models
{
    public class ServiceViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Médico Chefe")]
        public Guid DoctorId { get; set; }
        [ScaffoldColumn(false)]
        public string DoctorName { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Serviço")]
        [StringLength(50, ErrorMessage = "O campo {0} tem de ter no mínimo {2} caracteres.", MinimumLength = 2)]
        public string ServiceName { get; set; }
        public Guid ServiceId { get; set; }
        public bool? IsActive { get; set; }

        [ScaffoldColumn(false)]
        public string Ativo { get; set; }

        public IEnumerable<DoctorViewModel> ListOfDoctors { get; set; }
        [Required(ErrorMessage = "Tem de seleccionar no mínimo uma pessoa.")]
        [Display(Name = "Pessoal do Serviço")]
        public ICollection<Guid> ServiceDoctors { get; set; }

    }
}