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
    public class ObservationService : Service, IObservationService
    {
        private readonly HttpClient _httpClient;

        public ObservationService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);
        }

        public async Task<IEnumerable<ObservationViewModel>> GetAll(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/observations/get-all/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<ObservationViewModel>>(response);
        }

        public async Task<IEnumerable<ObservationViewModel>> GetAllExceptDoctor(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/observations/get-all-except-doctor/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<ObservationViewModel>>(response);
        }

        public async Task<ObservationViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/observations/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<ObservationViewModel>(response);
        }

        public async Task<ResponseResult> Create(ObservationViewModel observationViewModel)
        {
            var observationContent = GetContent(observationViewModel);

            var response = await _httpClient.PostAsync("/api/v1/observations", observationContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Update(ObservationViewModel observationViewModel)
        {
            var observationContent = GetContent(observationViewModel);

            var id = observationViewModel.Id;

            var response = await _httpClient.PutAsync($"/api/v1/observations/{id}", observationContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/observations/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
