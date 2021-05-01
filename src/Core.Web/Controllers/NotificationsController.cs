using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Web.Extensions;
using Core.Web.Models;
using Core.Web.Resource;
using Core.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Core.Web.Controllers
{



    public class NotificationsController : Controller
    {
        private readonly ICarePlanService _carePlanService;
        private readonly IMessageService _messageService;
        private readonly IInquiryService _inquiryService;
        private readonly INoticeService _noticeService;
        private readonly IStringLocalizer<Resources> _localizerGeneral;
        private readonly IAspNetUser _aspNetUser;
        private int total = 0;

        public NotificationsController(ICarePlanService carePlanService,
                                   IMessageService messageService,
                                   IInquiryService inquiryService,
                                   INoticeService noticeService,
                                   IStringLocalizer<Resources> localizerGeneral,
                                   IAspNetUser aspNetUser)
        {
            _carePlanService = carePlanService;
            _messageService = messageService;
            _inquiryService = inquiryService;
            _noticeService = noticeService;
            _localizerGeneral = localizerGeneral;
            _aspNetUser = aspNetUser;
        }

        public async Task<ActionResult> Index()
        {
            List<NotificationViewModel> response = new List<NotificationViewModel>();

            response.Add(await GetInqueriesPatientCounter());
            response.Add(await GetCarePlansPatientCounter());
            response.Add(await GetAllAwaitingResponseMessagesToPatientCounter());
            response.Add(await GetWarnings());
            response.Add(await GetAllCurrentNoticeBy());
            response.Add(await GetAllNotReadMessagesCounter());
            response.Add(new NotificationViewModel
            {
                Key = "total",
                LabelFormatted = string.Format(_localizerGeneral["{0} Notificações"], total),
                Value = total
            });

            return Json(response);
        }

        private async Task<NotificationViewModel> GetCarePlansPatientCounter()
        {
            var response = new NotificationViewModel { Key = "careplan", Value = 0, Icon = "fa-book-medical" };

            if (_aspNetUser.GetUserRole("paciente"))
            {
                var careplans = await _carePlanService.GetAllCarePlansByPacientId();
                var count = careplans.Count();
                if (careplans.Any() && count > 0)
                {
                    total += count;
                    response.Value = count;
                    response.LabelFormatted = string.Format(_localizerGeneral["{0} Plano de Cuidados"], count);
                    response.ActionUrl = "/CarePlans/PatientCarePlans";
                }
            }

            return response;
        }

        private async Task<NotificationViewModel> GetAllAwaitingResponseMessagesToPatientCounter()
        {
            var response = new NotificationViewModel { Key = "message", Value = 0, Icon = "fa-comments" };

            if (_aspNetUser.GetUserRole("paciente"))
            {
                var messages = await _messageService.GetAllAwaitingResponseMessagesToPatient();
                var count = messages.Count();
                if (messages.Any() && count > 0)
                {                    
                    response.Value = count;
                    response.LabelFormatted = string.Format(_localizerGeneral["{0} Mensagens por responder"], count);
                    response.ActionUrl = "/Messages/PatientMessages";
                    total += count;
                }
            }

            return response;
        }

        private async Task<NotificationViewModel> GetWarnings()
        {
            var l = new List<string>();
            var response = new NotificationViewModel { Key = "warnings", Value = 0, Icon = "bi-exclamation-triangle" };

            if (_aspNetUser.GetUserRole("medico"))
            {
                //TODO: Refatorar, esta regra de negocios deveria estar na business layer.
                var counter = GetAllAwaitingResponseMessagesToPatientCounter().Result.Value;
                if (counter != null && (int)counter > 0)
                {
                    total -= (int)counter;
                    var warnings = new List<string>();
                    warnings.Add(string.Format(_localizerGeneral["Atenção: Há {0} mensagens respondidas que ainda não foram lidas."], counter));
                    response.Value = warnings;
                    response.LabelFormatted = string.Empty; //nao se aplica
                    response.ActionUrl = "#";
                }
            }

            return response;
        }

        private async Task<NotificationViewModel> GetInqueriesPatientCounter()
        {
            var response = new NotificationViewModel { Key = "inquiries", Value = 0, Icon = "fa-calendar-check" };            

            if (_aspNetUser.GetUserRole("paciente"))
            {
                var inquiries = await _inquiryService.GetAllInquiryScheduleByPatientAnsweredOrNot(false);
                var count = inquiries.Count();
                if (inquiries.Any() && count > 0)
                {
                    total += count;
                    response.Value = count;
                    response.LabelFormatted = string.Format(_localizerGeneral["{0} Formulários Novos"], count);
                    response.ActionUrl = "/Inquiries/PatientInquiries";
                }
            }

            return response;
        }

        private async Task<NotificationViewModel> GetAllCurrentNoticeBy()
        {
            var response = new NotificationViewModel { Key = "notice", Value = 0, Icon = "fa-bell" };

            if (_aspNetUser.GetUserRole("paciente"))
            {
                var userNotices = await _noticeService.GetAllCurrentNoticeBy();
                var count = userNotices.Count();
                if (userNotices.Any() && count > 0)
                {
                    total += count;
                    response.Value = count;
                    response.LabelFormatted = string.Format(_localizerGeneral["{0} Avisos"], count);
                    response.ActionUrl = "/Notices/IndexByPatient";
                }
            }

            return response;
        }

        private async Task<NotificationViewModel> GetAllNotReadMessagesCounter()
        {
            var response = new NotificationViewModel { Key = "message", Value = 0, Icon = "fa-envelope" };

            if (_aspNetUser.GetUserRole("paciente"))
            {
                var messages = await _messageService.GetAllNotReadMessagesToPatient();
                var count = messages.Count();
                if (messages.Any() && count > 0)
                {                    
                    response.Value = count;
                    response.LabelFormatted = string.Format(_localizerGeneral["{0} Mensagens não lidas"], count);
                    response.ActionUrl = "/Messages/PatientMessages";
                    total += count;
                }
            }

            if (_aspNetUser.GetUserRole("medico"))
            {
                var messages = await _messageService.GetAllNotReadMessagesToDoctor();
                var count = messages.Count();
                if (messages.Any() && count > 0)
                {
                    HttpContext.Session.SetString("ActiveTabMessage", "ReplyMessages");
                    response.Value = count;
                    response.LabelFormatted = string.Format(_localizerGeneral["{0} Mensagens não lidas"], count);
                    response.ActionUrl = "/Messages";
                    total += count;
                }
            }

            return response;
        }
    }
}
