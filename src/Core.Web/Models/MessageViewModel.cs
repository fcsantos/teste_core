using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Web.Models
{
    public class MessageViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(5000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        [Display(Name = "Mensagem")]
        public string Text { get; set; }

        public Guid? ReplyMessageId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Paciente")]
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public string StatusMessage { get; set; }
        public bool? IsActive { get; set; }

        [Display(Name = "Aguardar a resposta do Paciente?")]
        public bool IsReply { get; set; }

        public bool IsMailSender { get; set; }

        [ScaffoldColumn(false)]
        public string PatientName { get; set; }

        [ScaffoldColumn(false)]
        public string Ativo { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Resposta")]
        public IEnumerable<MessageViewModel> ReplyMessages { get; set; }

        [ScaffoldColumn(false)]
        public IEnumerable<MessageViewModel> SentMessages { get; set; }

        [ScaffoldColumn(false)]
        public IEnumerable<MessageViewModel> ReplyMessagesPatient { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Data")]
        public string DateFormat { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Médico")]
        public string DoctorName { get; set; }

        [ScaffoldColumn(false)]
        public string StatusMessageFormat { get; set; }
    }
}
