using Core.Web.Extensions;
using Core.Web.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.Web.Services
{
    public class InquiryService : Service, IInquiryService
    {
        private readonly HttpClient _httpClient;

        public InquiryService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);
        }

        public async Task<IEnumerable<InquiryViewModel>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/v1/inquiries");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<InquiryViewModel>>(response);
        }

        public async Task<InquiryViewModel> GetByInquiryId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/inquiries/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<InquiryViewModel>(response);
        }

        public async Task<IEnumerable<ComboViewModel>> ComboInquiries()
        {
            var response = await _httpClient.GetAsync("/api/v1/inquiries/combo-inquiries");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<ComboViewModel>>(response);
        }

        public async Task<InquiryScheduleViewModel> GetByInquiryScheduleId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/inquiries-schedule/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<InquiryScheduleViewModel>(response);
        }

        public async Task<ResponseResult> CreateInquiry(InquiryViewModel inquiry)
        {
            inquiry.IsActive = true;
            var inquiryContent = GetContent(inquiry);

            var response = await _httpClient.PostAsync("/api/v1/inquiries", inquiryContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> CreateInquirySchedule(InquiryScheduleViewModel inquirySchedule)
        {
            var inquiryContent = GetContent(inquirySchedule);

            var response = await _httpClient.PostAsync("/api/v1/inquiries-schedule", inquiryContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> RespondInquiry(PatientAnswersViewModel patientAnswers)
        {
            var patientAnswersContent = GetContent(patientAnswers);

            var response = await _httpClient.PostAsync("/api/v1/patient-answers", patientAnswersContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> UpdateInquiry(InquiryViewModel inquiry)
        {
            var inquiryContent = GetContent(inquiry);

            var id = inquiry.Id;

            var response = await _httpClient.PutAsync($"/api/v1/inquiries/{id}", inquiryContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> UpdateInquiryScheduleAnswered(InquiryScheduleViewModel inquiryScheduleViewModel)
        {
            var inquiryScheduleContent = GetContent(inquiryScheduleViewModel);

            var id = inquiryScheduleViewModel.Id;
            inquiryScheduleViewModel.answered = true;

            var response = await _httpClient.PutAsync($"/api/v1/inquiries-schedule/{id}", inquiryScheduleContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<IEnumerable<InquiryScheduleViewModel>> GetAllInquiryScheduleByPatientUserId()
        {
            var response = await _httpClient.GetAsync($"/api/v1/inquiries-schedule/get-all-bypatientuserid");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<InquiryScheduleViewModel>>(response);
        }

        public async Task<IEnumerable<InquiryScheduleViewModel>> GetAllInquiryScheduleByPatientId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/inquiries-schedule/get-all-bypatientid/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<InquiryScheduleViewModel>>(response);
        }

        public async Task<IEnumerable<InquiryScheduleViewModel>> GetAllInquiryScheduleByPatientAnsweredOrNot(bool answered)
        {
            var response = await _httpClient.GetAsync($"/api/v1/inquiries-schedule/get-all-bypatient-answered-or-not/{answered}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<InquiryScheduleViewModel>>(response);
        }

        public async Task<IEnumerable<PatientAnswersViewModel>> GetPatientAnswersByInquiryScheduleId(Guid inquiryScheduleId)
        {
            var response = await _httpClient.GetAsync($"/api/v1/patient-answers/answers-byinquiryschedule/{inquiryScheduleId}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<PatientAnswersViewModel>>(response);
        }

    }
}
