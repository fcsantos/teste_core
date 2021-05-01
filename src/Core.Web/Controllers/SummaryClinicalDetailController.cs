using System;
using System.Threading.Tasks;
using Core.Web.Extensions;
using Core.Web.Models;
using Core.Web.Resource;
using Core.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace Core.Web.Controllers
{
    public class SummaryClinicalDetailController : MainController
    {
        private readonly IConsultationService _consultationService;
        private readonly IAllergyService _allergyService;
        private readonly IDiagnosticService _diagnosticService;
        private readonly ICarePlanService _carePlanService;
        private readonly IObservationService _observationService;
        private readonly IPatientService _patientService;
        private readonly IClinicalSummaryFacilitatorService _clinicalSummaryFacilitatorService;
        private readonly IPathologyService _pathologyService;
        private readonly IMessageService _messageService;
        private readonly IInquiryService _inquiryService;
        private readonly IStringLocalizer<SummaryClinicalDetailController> _localizer;
        private readonly IStringLocalizer<Resources> _localizerGeneral;

        public SummaryClinicalDetailController(IConsultationService consultationService,
                                               IAllergyService allergyService,
                                               IDiagnosticService diagnosticService,
                                               ICarePlanService carePlanService,
                                               IObservationService observationService,
                                               IPatientService patientService,
                                               IClinicalSummaryFacilitatorService clinicalSummaryFacilitatorService,
                                               IPathologyService pathologyService,
                                               IMessageService messageService,
                                               IInquiryService inquiryService,
                                               IStringLocalizer<SummaryClinicalDetailController> localizer,
                                               IStringLocalizer<Resources> localizerGeneral)
        {
            _consultationService = consultationService;
            _allergyService = allergyService;
            _diagnosticService = diagnosticService;
            _carePlanService = carePlanService;
            _observationService = observationService;
            _patientService = patientService;
            _clinicalSummaryFacilitatorService = clinicalSummaryFacilitatorService;
            _pathologyService = pathologyService;
            _messageService = messageService;
            _inquiryService = inquiryService;
            _localizer = localizer;
            _localizerGeneral = localizerGeneral;
        }

        [ClaimsAuthorize("SummaryClinicalDetail", "General")]
        public async Task<IActionResult> Index(Guid id)
        {
            ViewData["ActiveTab"] = HttpContext.Session.GetString("ActiveTab");

            var patientViewModel = await _patientService.GetById(id);

            if (patientViewModel == null) return NotFound();

            ViewData["SummaryClinicalDetailPatient"] = _localizer["Resumo Clínico do Paciente: {0}", patientViewModel.Document + " - " + patientViewModel.FirstName + " " + patientViewModel.LastName];

            ViewData["SelectListFacilitatorAllergy"] = new SelectList(_clinicalSummaryFacilitatorService.Combo("Allergy").Result, "Id", "Name");
            ViewData["SelectListFacilitatorDiagnostic"] = new SelectList(_clinicalSummaryFacilitatorService.Combo("Diagnostic").Result, "Id", "Name");
            ViewData["SelectListFacilitatorObservation"] = new SelectList(_clinicalSummaryFacilitatorService.Combo("Observation").Result, "Id", "Name");

            ViewData["SelectListPathologies"] = new SelectList(_pathologyService.ComboPathologies().Result, "Id", "Name");
            ViewData["SelectListInquiries"] = new SelectList(_inquiryService.ComboInquiries().Result, "Id", "Name");

            ViewData["PatientId"] = patientViewModel.Id;

            return View(new SummaryClinicalDetailViewModel
            {
                Consultations = await _consultationService.GetAll(id),
                Consultation = new ConsultationViewModel { PatientId = patientViewModel.Id },
                ConsultationsExcept = await _consultationService.GetAllExceptDoctor(id),
                Allergies = await _allergyService.GetAll(id),
                Allergy = new AllergyViewModel { PatientId = patientViewModel.Id },
                AllergiesExcept = await _allergyService.GetAllExceptDoctor(id),
                Diagnostics = await _diagnosticService.GetAll(id),
                Diagnostic = new DiagnosticViewModel { PatientId = patientViewModel.Id },
                DiagnosticsExcept = await _diagnosticService.GetAllExceptDoctor(id),
                CarePlans = await _carePlanService.GetAll(id),
                CarePlan = new CarePlanViewModel { PatientId = patientViewModel.Id },
                CarePlansExcept = await _carePlanService.GetAllExceptDoctor(id),
                Observations = await _observationService.GetAll(id),
                Observation = new ObservationViewModel { PatientId = patientViewModel.Id },
                ObservationsExcept = await _observationService.GetAllExceptDoctor(id),
                Messages = await _messageService.GetAllMessagesByPacientId(id),
                Message = new MessageViewModel { PatientId = patientViewModel.Id },
                InquiriesSchedule = await _inquiryService.GetAllInquiryScheduleByPatientId(id)
            });
        }

        [ClaimsAuthorize("SummaryClinicalDetail", "General")]
        [HttpGet]
        public async Task<JsonResult> GetFacilitatorDescriptionById(string id)
        {
            var facilitatorDescription = await _clinicalSummaryFacilitatorService.GetById(Guid.Parse(id));

            return Json(facilitatorDescription.Description);
        }
    }
}
