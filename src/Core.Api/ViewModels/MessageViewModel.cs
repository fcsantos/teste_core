using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Api.ViewModels
{
    public class MessageViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(5000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Text { get; set; }

        public Guid? ReplyMessageId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public string StatusMessage { get; set; }
        public bool? IsActive { get; set; }
        public bool IsReply { get; set; }

        public bool IsMailSender { get; set; }

        [ScaffoldColumn(false)]
        public string PatientName { get; set; }

        [ScaffoldColumn(false)]
        public string Ativo { get; set; }

        [ScaffoldColumn(false)]
        public string DateFormat { get; set; }

        [ScaffoldColumn(false)]
        public string DoctorName { get; set; }

        [ScaffoldColumn(false)]
        public string StatusMessageFormat { get; set; }
    }
}
