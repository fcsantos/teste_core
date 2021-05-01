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
    public class DashboardService : Service, IDashboardService
    {
        private readonly HttpClient _httpClient;

        public DashboardService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);
        }

        public async Task<int?> GetAllActiveInquiries()
        {
            var response = await _httpClient.GetAsync("/api/v1/dashboard/get-all-active-inquiries");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<int>(response);
        }

        public async Task<int?> GetAllActivePatients()
        {
            var response = await _httpClient.GetAsync("/api/v1/dashboard/get-all-active-patients");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<int>(response);
        }

        public async Task<int?> GetAllConsultantByDoctorId()
        {
            var response = await _httpClient.GetAsync("/api/v1/dashboard/get-all-consultant-by-doctorid");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<int>(response);
        }

        public async Task<int?> GetAllDiagnosticByDoctorId()
        {
            var response = await _httpClient.GetAsync("/api/v1/dashboard/get-all-diagnostic-by-doctorid");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<int>(response);
        }

        public async Task<IEnumerable<InquiryScheduleViewModel>> GetAllInquiryAnswered()
        {
            var response = await _httpClient.GetAsync($"/api/v1/dashboard/get-all-inquiry-answered");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<InquiryScheduleViewModel>>(response);
        }

        public async Task<IEnumerable<MessageViewModel>> GetAllAnsweredMessages()
        {
            var response = await _httpClient.GetAsync($"/api/v1/dashboard/get-all-message-answered");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<MessageViewModel>>(response);
        }
    }
}
