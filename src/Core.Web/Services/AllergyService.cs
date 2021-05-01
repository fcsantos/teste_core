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
    public class AllergyService : Service, IAllergyService
    {
        private readonly HttpClient _httpClient;

        public AllergyService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);
        }

        public async Task<IEnumerable<AllergyViewModel>> GetAll(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/allergies/get-all/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<AllergyViewModel>>(response);
        }

        public async Task<IEnumerable<AllergyViewModel>> GetAllExceptDoctor(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/allergies/get-all-except-doctor/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<AllergyViewModel>>(response);
        }

        public async Task<AllergyViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/allergies/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<AllergyViewModel>(response);
        }

        public async Task<ResponseResult> Create(AllergyViewModel allergyViewModel)
        {
            var allergyContent = GetContent(allergyViewModel);

            var response = await _httpClient.PostAsync("/api/v1/allergies", allergyContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Update(AllergyViewModel allergyViewModel)
        {
            var allergyContent = GetContent(allergyViewModel);

            var id = allergyViewModel.Id;

            var response = await _httpClient.PutAsync($"/api/v1/allergies/{id}", allergyContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/allergies/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
