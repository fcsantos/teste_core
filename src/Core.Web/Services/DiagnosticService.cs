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
    public class DiagnosticService : Service, IDiagnosticService
    {
        private readonly HttpClient _httpClient;

        public DiagnosticService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);
        }

        public async Task<IEnumerable<DiagnosticViewModel>> GetAll(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/diagnostics/get-all/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<DiagnosticViewModel>>(response);
        }

        public async Task<IEnumerable<DiagnosticViewModel>> GetAllExceptDoctor(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/diagnostics/get-all-except-doctor/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<DiagnosticViewModel>>(response);
        }

        public async Task<DiagnosticViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/diagnostics/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<DiagnosticViewModel>(response);
        }

        public async Task<ResponseResult> Create(DiagnosticViewModel diagnosticViewModel)
        {
            var diagnosticContent = GetContent(diagnosticViewModel);

            var response = await _httpClient.PostAsync("/api/v1/diagnostics", diagnosticContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Update(DiagnosticViewModel diagnosticViewModel)
        {
            var diagnosticContent = GetContent(diagnosticViewModel);

            var id = diagnosticViewModel.Id;

            var response = await _httpClient.PutAsync($"/api/v1/diagnostics/{id}", diagnosticContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/diagnostics/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
