using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Web.Extensions;
using Core.Web.Models;
using Microsoft.Extensions.Options;

namespace Core.Web.Services
{
    public class ConsultationService : Service, IConsultationService
    {
        private readonly HttpClient _httpClient;

        public ConsultationService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);

        }

        public async Task<IEnumerable<ConsultationViewModel>> GetAll(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/consultations/get-all/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<ConsultationViewModel>>(response);
        }

        public async Task<IEnumerable<ConsultationViewModel>> GetAllExceptDoctor(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/consultations/get-all-except-doctor/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<ConsultationViewModel>>(response);
        }

        public async Task<ConsultationViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/consultations/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<ConsultationViewModel>(response);
        }

        public async Task<ResponseResult> Create(ConsultationViewModel consultationViewModel)
        {
            var consultationContent = GetContent(consultationViewModel);

            var response = await _httpClient.PostAsync("/api/v1/consultations", consultationContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Update(ConsultationViewModel consultationViewModel)
        {
            var consultationContent = GetContent(consultationViewModel);

            var id = consultationViewModel.Id;

            var response = await _httpClient.PutAsync($"/api/v1/consultations/{id}", consultationContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/consultations/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
