using System;
using System.Linq;
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
    public class MessagesController : MainController
    {
        private readonly IMessageService _messageService;
        private readonly IPatientService _patientService;
        private readonly IStringLocalizer<Resources> _localizerGeneral;
        private readonly IStringLocalizer<MessagesController> _localizer;

        public MessagesController(IMessageService messageService,
                                  IPatientService patientService,
                                  IStringLocalizer<Resources> localizerGeneral,
                                  IStringLocalizer<MessagesController> localizer)
        {
            _messageService = messageService;
            _patientService = patientService;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;
        }

        #region Doctor
        [ClaimsAuthorize("Message", "GetByDoctor")]
        public async Task<IActionResult> Index()
        {
            ViewData["ActiveTabMessage"] = HttpContext.Session.GetString("ActiveTabMessage");

            var messageViewModel = new MessageViewModel
            {
                ReplyMessages = await _messageService.GetAllMessagesReplyByDoctorId(),
                SentMessages = await _messageService.GetAllMessagesSentByDoctorId()
            };

            return View(messageViewModel);
        }

        [ClaimsAuthorize("Message", "Create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["SelectListPatient"] = new SelectList(_patientService.Combo().Result, "Id", "Name");

            return View();
        }

        [ClaimsAuthorize("Message", "Create")]
        [HttpPost]
        public async Task<IActionResult> Create(MessageViewModel messageViewModel)
        {
            HttpContext.Session.SetString("ActiveTabMessage", "SentMessages");

            var response = await _messageService.Create(messageViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();


            return RedirectToAction("Index", "Messages");
        }

        [ClaimsAuthorize("Message", "Update")]
        [HttpPost]
        public ActionResult EditPost(Guid Id)
        {
            HttpContext.Session.SetString("ActiveTabMessage", "ReplyMessages");

            var messageViewModel = _messageService.GetById(Id).Result;

            if (messageViewModel == null) return NotFound();

            var response = _messageService.Update(messageViewModel).Result;

            if (response.Errors.Messages.Count() > 0)
                return Json(new DeleteResponseMessage { message = response.Errors.Messages.FirstOrDefault(), success = false });

            return Json(response);
        }

        [ClaimsAuthorize("Message", "GetByDoctor")]
        [HttpPost]
        public ActionResult Detail(Guid id)
        {
            var messageViewModel = _messageService.GetById(id).Result;
            if (messageViewModel == null) return NotFound();
            return Json(new MessageViewModel { Text = messageViewModel.Text });
        }

        [ClaimsAuthorize("Message", "CreateByDoctor")]
        [HttpPost]
        public async Task<IActionResult> CreateMessage(SummaryClinicalDetailViewModel summaryClinicalDetailViewModel)
        {
            HttpContext.Session.SetString("ActiveTab", "Message");

            var response = await _messageService.Create(summaryClinicalDetailViewModel.Message);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();


            return RedirectToAction("Index", "SummaryClinicalDetail", new { id = summaryClinicalDetailViewModel.Message.PatientId });
        }
        #endregion

        #region Patient
        [ClaimsAuthorize("Message", "GetByPacient")]
        public async Task<IActionResult> PatientMessages()
        {
            return View(new MessageViewModel { ReplyMessagesPatient = await _messageService.GetAllMessagesByPacientId() });
        }

        [ClaimsAuthorize("Message", "GetBy")]
        public async Task<ActionResult> GetMessageById(Guid id)
        {
            var messageViewModel = await _messageService.GetByIdWithReplyMessage(id);

            if (messageViewModel == null) return NotFound();

            return Json(new MessageViewModel
            {
                Id = messageViewModel.Id,
                Text = messageViewModel.Text,
                IsReply = messageViewModel.IsReply,
                DoctorId = messageViewModel.DoctorId,
                PatientId = messageViewModel.PatientId,
                PatientName = messageViewModel.PatientName,
                DoctorName = messageViewModel.DoctorName,
                DateFormat = messageViewModel.DateFormat,
                ReplyMessageId = messageViewModel.ReplyMessageId,
                StatusMessage = messageViewModel.StatusMessage
            });
        }

        [ClaimsAuthorize("Message", "GetBy")]
        public async Task<ActionResult> GetByReplyMessageId(Guid id)
        {
            var messageViewModel = await _messageService.GetByReplyMessageId(id);
            if (messageViewModel == null) return NotFound();

            return Json(new MessageViewModel
            {
                Id = messageViewModel.Id,
                Text = messageViewModel.Text,
                IsReply = messageViewModel.IsReply,
                DoctorId = messageViewModel.DoctorId,
                PatientId = messageViewModel.PatientId,
                ReplyMessageId = messageViewModel.ReplyMessageId,
                PatientName = messageViewModel.PatientName,
                DoctorName = messageViewModel.DoctorName,
                DateFormat = messageViewModel.DateFormat,
                StatusMessage = messageViewModel.StatusMessage
            });
        }

        [ClaimsAuthorize("Message", "Create")]
        [HttpPost]
        public async Task<IActionResult> ReplyMessage(MessageViewModel messageViewModel)
        {
            var response = await _messageService.Create(messageViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            return RedirectToAction("PatientMessages", "Messages");
        }

        [ClaimsAuthorize("Message", "Update")]
        [HttpPost]
        public async Task<IActionResult> PatientEditPost(Guid Id)
        {
            var messageViewModel = await _messageService.GetById(Id);

            if (messageViewModel == null) return NotFound();

            var response = await _messageService.Update(messageViewModel);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            return RedirectToAction("PatientMessages", "Messages");
        }
        #endregion
    }
}
