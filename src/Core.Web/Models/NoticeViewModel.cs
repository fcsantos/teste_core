using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Web.Models
{
    public class NoticeViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Data de Início"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[Display(Name = "Data de Início")]
        //[Required(ErrorMessage = "O campo {0} é obrigatório")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //[RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d+$", ErrorMessage = "O campo {0} está em formato inválido")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Data de Fim"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[Display(Name = "Data de Fim")]
        //[Required(ErrorMessage = "O campo {0} é obrigatório")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //[RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d+$", ErrorMessage = "O campo {0} está em formato inválido")]
        public DateTime EndDate { get; set; }

        [ScaffoldColumn(false)]
        public string PatientsName { get; set; }

        public bool? IsActive { get; set; }

        [ScaffoldColumn(false)]
        public string Ativo { get; set; }

        [Display(Name = "Enviar para todos os utilizadores")]
        public bool SendToAllUsers { get; set; }

        public string Status { get; set; }

        [Display(Name = "Pacientes")]
        public ICollection<PatientViewModel> ListOfPatients { get; set; }
                
        public ICollection<Guid> NoticeUsers { get; set; }

        [ScaffoldColumn(false)]
        public string StartDateFormat { get; set; }

        [ScaffoldColumn(false)]
        public string EndDateFormat { get; set; }
    }

    public class UserNoticeViewModel
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool SendToAllUsers { get; set; }
        public string Status { get; set; }
    }
}