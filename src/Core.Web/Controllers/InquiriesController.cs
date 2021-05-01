using Core.Web.Extensions;
using Core.Web.Models;
using Core.Web.Resource;
using Core.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Controllers
{
    public class InquiriesController : MainController
    {
        private readonly IInquiryService _inquiryService;
        private readonly IStringLocalizer<Resources> _localizerGeneral;
        private readonly IStringLocalizer<InquiriesController> _localizer;

        public InquiriesController(IInquiryService pathologyService,
                                     IStringLocalizer<Resources> localizerGeneral,
                                     IStringLocalizer<InquiriesController> localizer)
        {
            _inquiryService = pathologyService;
            _localizerGeneral = localizerGeneral;
            _localizer = localizer;
        }

        [ClaimsAuthorize("Inquiry", "Get")]
        public async Task<IActionResult> Index()
        {
            return View(await _inquiryService.GetAll());
        }

        [ClaimsAuthorize("Inquiry", "Get")]
        public async Task<IActionResult> PatientInquiries()
        {
            return View(await _inquiryService.GetAllInquiryScheduleByPatientUserId());
        }

        [ClaimsAuthorize("Inquiry", "Get")]
        [HttpGet("{id:guid}/PatientInquiries")]
        public async Task<IActionResult> PatientInquiries(Guid id)
        {
            return View(await _inquiryService.GetAllInquiryScheduleByPatientId(id));
        }

        [ClaimsAuthorize("Inquiry", "Get")]
        [HttpGet]
        public async Task<IActionResult> GetPatientInquiriesAnswered()
        {
            return View(await _inquiryService.GetAllInquiryScheduleByPatientAnsweredOrNot(true));
        }

        [ClaimsAuthorize("Inquiry", "Get")]
        public async Task<IActionResult> Details(Guid id)
        {
            var inquiryShedule = await _inquiryService.GetByInquiryScheduleId(id);
            var inquiryViewModel = await _inquiryService.GetByInquiryId(inquiryShedule.InquiryId);

            inquiryViewModel.InquiryScheduleId = inquiryShedule.Id;

            if (inquiryViewModel == null) return NotFound();

            ViewData["EditInquiryName"] = _localizer["Inquérito : {0}", inquiryViewModel.Title];

            return View(inquiryViewModel);
        }
        public ActionResult MainView()
        {
            return View();
        }
        public ActionResult AddQuestionPartialView()
        {
            QuestionViewModel modelQ = new QuestionViewModel();
            return PartialView("_AddQuestionPartialView", modelQ);
        }
        public ActionResult AddAnswerPartialView()
        {
            AnswerOptionsViewModel modelQ = new AnswerOptionsViewModel();
            return PartialView("_AddAnswerPartialView", modelQ);
        }

        [ClaimsAuthorize("Inquiry", "Get")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [ClaimsAuthorize("Inquiry", "Get")]
        public async Task<IActionResult> PatientAnswers(Guid id)
        {
            InquiryResultViewModel inquiryResult = new InquiryResultViewModel();
            var patientAnswers = await _inquiryService.GetPatientAnswersByInquiryScheduleId(id);
            var inquiry = await _inquiryService.GetByInquiryScheduleId(id);

            //inquiryResult.InquiryTitle = inquiry.Inquiry.Title;
            //inquiryResult.InquiryDescription = inquiry.Inquiry.Description;
            //inquiryResult.PatientName = inquiry.Patient.FirstName + " " + inquiry.Patient.LastName;
            //inquiryResult.DoctorName = inquiry.Doctor.Name;
            
            inquiryResult.PatientAnswers = patientAnswers;
            if (inquiry.UpdatedDate.HasValue)
            {
                inquiryResult.UpdatedDate = inquiry.UpdatedDate.Value;
            }


            foreach (var item in patientAnswers)
            {
                inquiryResult.InquiryTitle = item.InquiryTitle;
                inquiryResult.AnswerValueSum += item.AnswerValue;
            }

            return View(inquiryResult);
        }


        [ClaimsAuthorize("Inquiry", "CreateByDoctor")]
        [HttpPost]
        public async Task<IActionResult> CreateInquirySchedule(SummaryClinicalDetailViewModel summaryClinicalDetailViewModel)
        {
            HttpContext.Session.SetString("ActiveTab", "Inquiry");

            var response = await _inquiryService.CreateInquirySchedule(summaryClinicalDetailViewModel.InquirySchedule);

            if (ResponseHasErrors(response)) TempData["Erros"] =
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();


            return RedirectToAction("Index", "SummaryClinicalDetail", new { id = summaryClinicalDetailViewModel.InquirySchedule.PatientId });
        }


        [ClaimsAuthorize("Inquiry", "Get")]
        [HttpPost]
        public async Task<IActionResult> Create(Dictionary<string, string> model)
        {
            ResponseResult response = new ResponseResult();

            var InquiryScheduleId = Guid.Parse(model["InquiryScheduleId"]);
            var inquiryScheduleViewModel = await _inquiryService.GetByInquiryScheduleId(InquiryScheduleId);

            var InquiryId = Guid.Parse(model["InquiryId"]);
            var inquiryViewModel = await _inquiryService.GetByInquiryId(InquiryId);

            if (inquiryViewModel == null || inquiryScheduleViewModel == null) return NotFound();

            PatientAnswersViewModel patientAnswersViewModel = new PatientAnswersViewModel();

            foreach (var question in inquiryViewModel.Questions)
            {
                foreach (KeyValuePair<string, string> line in model)
                {
                    patientAnswersViewModel.InquiryId = InquiryId;
                    patientAnswersViewModel.InquiryScheduleId = InquiryScheduleId;
                    patientAnswersViewModel.InquiryTitle = inquiryViewModel.Title;
                    patientAnswersViewModel.QuestionId = question.Id;
                    patientAnswersViewModel.QuestionTitle = question.Title;

                    var answer = line.Key.Split("|");

                    if (answer == null) break;
                    if (answer[0].Equals("Q") && Guid.Parse(answer[1]) == question.Id)
                    {
                        if (answer[2].Equals("B")) //In RadioButton name have to be the same, so, B identifies answer option is in second position 
                        {
                            patientAnswersViewModel.AnswerOptionId = Guid.Parse(line.Value);
                            var answOption = question.AnswerOptions.Where(q => q.Id == Guid.Parse(line.Value)).FirstOrDefault();
                            patientAnswersViewModel.AnswerText = answOption.Option;
                            patientAnswersViewModel.AnswerValue = answOption.AnswerValue;
                            response = await _inquiryService.RespondInquiry(patientAnswersViewModel);
                        }
                        else
                        {
                            patientAnswersViewModel.AnswerOptionId = Guid.Parse(answer[2]);
                            if(line.Value != null)
                            {
                                patientAnswersViewModel.AnswerText = line.Value;
                                foreach (var value in question.AnswerOptions)
                                {
                                    if (value.QuestionId == question.Id && value.Id == Guid.Parse(answer[2]))
                                    {
                                        patientAnswersViewModel.AnswerValue = value.AnswerValue;
                                    }

                                }
                                response = await _inquiryService.RespondInquiry(patientAnswersViewModel);
                            }

                        }
                    }
                }

                if (ResponseHasErrors(response))
                {
                    TempData["Erros"] = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                }
                else
                {
                    var setInquiryToAnswered = await _inquiryService.UpdateInquiryScheduleAnswered(inquiryScheduleViewModel);
                }
            }
                return RedirectToAction("PatientAnswers", "Inquiries", new { id = InquiryScheduleId });
        }

            [ClaimsAuthorize("Inquiry", "Update")]
            [HttpGet("{id:guid}/EditInquiries")]
            public async Task<IActionResult> EditInquiry(Guid id)
            {
                var inquiryViewModel = await _inquiryService.GetByInquiryId(id);

                if (inquiryViewModel == null) return NotFound();

                ViewData["EditInquiryName"] = _localizer["Editar Inquérito : {0}", inquiryViewModel.Title];

                return View(inquiryViewModel);

            }

            [ClaimsAuthorize("Inquiry", "Update")]
            [HttpPost]
            public async Task<IActionResult> EditInquiry(InquiryViewModel inquiryViewModel)
            {
                var response = await _inquiryService.UpdateInquiry(inquiryViewModel);

                if (ResponseHasErrors(response)) TempData["Erros"] =
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

                return RedirectToAction("Index", "Inquiries");
            }

        }
    }
