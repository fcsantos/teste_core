using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public class SummaryClinicalDetailViewModel
    {
        public IEnumerable<ConsultationViewModel> Consultations { get; set; }

        public IEnumerable<ConsultationViewModel> ConsultationsExcept { get; set; }

        public ConsultationViewModel Consultation { get; set; }

        public IEnumerable<AllergyViewModel> Allergies { get; set; }

        public IEnumerable<AllergyViewModel> AllergiesExcept { get; set; }

        public AllergyViewModel Allergy { get; set; }

        public ClinicalSummaryFacilitatorViewModel Facilitator { get; set; }

        public IEnumerable<DiagnosticViewModel> Diagnostics { get; set; }

        public IEnumerable<DiagnosticViewModel> DiagnosticsExcept { get; set; }

        public DiagnosticViewModel Diagnostic { get; set; }

        public IEnumerable<CarePlanViewModel> CarePlans { get; set; }

        public IEnumerable<CarePlanViewModel> CarePlansExcept { get; set; }

        public CarePlanViewModel CarePlan { get; set; }

        public IEnumerable<ObservationViewModel> Observations { get; set; }

        public IEnumerable<ObservationViewModel> ObservationsExcept { get; set; }

        public ObservationViewModel Observation { get; set; }

        public bool IsNewFacilitator { get; set; }

        public Guid CloneId { get; set; }

        public string CloneDescription { get; set; }

        public IEnumerable<MessageViewModel> Messages { get; set; }

        public MessageViewModel Message { get; set; }

        public MessageViewModel MessagesReply { get; set; }
        public IEnumerable<InquiryScheduleViewModel> InquiriesSchedule { get; set; }
        public InquiryScheduleViewModel InquirySchedule { get; set; }
    }
}
