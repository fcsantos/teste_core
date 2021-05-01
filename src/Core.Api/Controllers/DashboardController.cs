using AutoMapper;
using Core.Api.Extensions;
using Core.Api.ViewModels;
using Core.Business.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/dashboard")]
    public class DashboardController : MainController
    {
        private readonly IInquiryRepository _inquiryRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IConsultationRepository _consultationRepository;
        private readonly IDiagnosticRepository _diagnosticRepository;
        private readonly IInquiryScheduleRepository _inquiryScheduleRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public DashboardController(IInquiryRepository inquiryRepository,
                                   IPatientRepository patientRepository,
                                   IConsultationRepository consultationRepository,
                                   IDiagnosticRepository diagnosticRepository,
                                   IInquiryScheduleRepository inquiryScheduleRepository,
                                   IMessageRepository messageRepository,
                                   IMapper mapper,
                                   INotifier notifier, IUser user) : base(notifier, user)
        {
            _inquiryRepository = inquiryRepository;
            _patientRepository = patientRepository;
            _consultationRepository = consultationRepository;
            _diagnosticRepository = diagnosticRepository;
            _inquiryScheduleRepository = inquiryScheduleRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }


        #region Doctor

        [ClaimsAuthorize("Dashboard", "GetByDoctor")]
        [HttpGet("get-all-active-inquiries")]
        public async Task<int> GetAllActiveInquiries()
        {
            return await _inquiryRepository.GetAllActiveCount();
        }

        [ClaimsAuthorize("Dashboard", "GetByDoctor")]
        [HttpGet("get-all-active-patients")]
        public async Task<int> GetAllActivePatients()
        {
            return await _patientRepository.GetAllActiveCount();
        }

        [ClaimsAuthorize("Dashboard", "GetByDoctor")]
        [HttpGet("get-all-consultant-by-doctorid")]
        public async Task<int> GetAllConsultantByDoctorId()
        {
            return await _consultationRepository.GetAllCountByDoctorId();
        }

        [ClaimsAuthorize("Dashboard", "GetByDoctor")]
        [HttpGet("get-all-diagnostic-by-doctorid")]
        public async Task<int> GetAllDiagnosticByDoctorId()
        {
            return await _diagnosticRepository.GetAllCountByDoctorId();
        }

        [ClaimsAuthorize("Dashboard", "GetByDoctor")]
        [HttpGet("get-all-inquiry-answered")]
        public async Task<IEnumerable<InquiryScheduleViewModel>> GetAllInquiryAnswered()
        {
            return _mapper.Map<IEnumerable<InquiryScheduleViewModel>>(await _inquiryScheduleRepository.GetAllByAnsweredOrNot(true));
        }

        [ClaimsAuthorize("Dashboard", "GetByDoctor")]
        [HttpGet("get-all-message-answered")]
        public async Task<IEnumerable<MessageViewModel>> GetAllAnsweredMessages()
        {
            return _mapper.Map<IEnumerable<MessageViewModel>>(await _messageRepository.GetAllNotReadMessagesToDoctor());
        }

        #endregion
    }
}
