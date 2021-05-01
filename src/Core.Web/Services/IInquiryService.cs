using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Web.Models;

namespace Core.Web.Services
{
    public interface IInquiryService
    {
        Task<IEnumerable<InquiryViewModel>> GetAll();
        Task<InquiryViewModel> GetByInquiryId(Guid inquiryId);
        Task<InquiryScheduleViewModel> GetByInquiryScheduleId(Guid inquiryScheduleId);
        Task<ResponseResult> CreateInquiry(InquiryViewModel inquiryViewModel);
        Task<ResponseResult> CreateInquirySchedule(InquiryScheduleViewModel inquiryScheduleViewModel);
        Task<ResponseResult> RespondInquiry(PatientAnswersViewModel patientAnswers);
        Task<ResponseResult> UpdateInquiry(InquiryViewModel inquiryViewModel);
        Task<ResponseResult> UpdateInquiryScheduleAnswered(InquiryScheduleViewModel inquiryScheduleViewModel);
        Task<IEnumerable<InquiryScheduleViewModel>> GetAllInquiryScheduleByPatientUserId();
        Task<IEnumerable<InquiryScheduleViewModel>> GetAllInquiryScheduleByPatientId(Guid id);
        Task<IEnumerable<InquiryScheduleViewModel>> GetAllInquiryScheduleByPatientAnsweredOrNot(bool answered);
        Task<IEnumerable<PatientAnswersViewModel>> GetPatientAnswersByInquiryScheduleId(Guid inquiryScheduleId);
        Task<IEnumerable<ComboViewModel>> ComboInquiries();
    }
}
