using System;
using System.ComponentModel.DataAnnotations;
namespace Core.Api.ViewModels
{
    public class InquiryScheduleViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid InquiryId { get; set; }
        public bool? answered { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime StartDate { get; set; }
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
