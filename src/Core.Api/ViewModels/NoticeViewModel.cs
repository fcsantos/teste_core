using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Core.Api.ViewModels
{
    public class NoticeViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Description { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime EndDate { get; set; }
        public bool? IsActive { get; set; }
        [ScaffoldColumn(false)]
        public string Ativo { get; set; }


        [ScaffoldColumn(false)]
        public string PatientsName { get; set; }

        public bool SendToAllUsers { get; set; }

        public string Status { get; set; }

        public ICollection<Guid> NoticeUsers { get; set; }

        [ScaffoldColumn(false)]
        public string StartDateFormat { get; set; }

        [ScaffoldColumn(false)]
        public string EndDateFormat { get; set; }
    }
}
