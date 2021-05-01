using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Web.Models
{
    public class InquiryScheduleViewModel
    {
        public Guid Id { get; set; }

        public Guid PatientId { get; set; }

        public Guid DoctorId { get; set; }

        public Guid InquiryId { get; set; }

        public bool? answered { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Data de Início"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Data Final"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? UpdatedDate { get; set; }

        [ScaffoldColumn(false)]
        public string PatientName { get; set; }

        [ScaffoldColumn(false)]
        public string InquiryTitle { get; set; }

        public bool IsMailSender { get; set; }

        public InquiryViewModel Inquiry { get; set; }
        public PatientViewModel Patient { get; set; }
        public DoctorViewModel Doctor { get; set; }
    }
}
