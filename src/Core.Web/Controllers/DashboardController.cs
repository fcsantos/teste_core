using Core.Web.Extensions;
using Core.Web.Resource;
using Core.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Core.Web.Controllers
{
    [Authorize]
    public class DashboardController : MainController
    {
        private readonly IDashboardService _dashboardService;
        private readonly IStringLocalizer<Resources> _localizerGeneral;
        private readonly IStringLocalizer<ClinicalSummaryFacilitatorsController> _localizer;
        private readonly IAspNetUser _aspNetUser;

        public DashboardController(IDashboardService dashboardService,
                                   IStringLocalizer<Resources> localizerGeneral,
                                   IStringLocalizer<ClinicalSummaryFacilitatorsController> localizer,
                                   IAspNetUser aspNetUser)
        {
            _dashboardService = dashboardService;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;
            _aspNetUser = aspNetUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (_aspNetUser.GetUserRole("medico"))
            {
                ViewData["ActiveInquiries"] = await _dashboardService.GetAllActiveInquiries();
                ViewData["ActivePatients"] = await _dashboardService.GetAllActivePatients();
                ViewData["ConsultantByDoctorId"] = await _dashboardService.GetAllConsultantByDoctorId();
                ViewData["DiagnosticByDoctorId"] = await _dashboardService.GetAllDiagnosticByDoctorId();
                ViewData["InqueriesAnswered"] = await _dashboardService.GetAllInquiryAnswered();
                ViewData["MessagesAnswered"] = await _dashboardService.GetAllAnsweredMessages();
            }

            return View();
        }
    }
}
