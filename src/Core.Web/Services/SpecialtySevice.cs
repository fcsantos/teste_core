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
    public class SpecialtySevice : Service, ISpecialtySevice
    {
        private readonly HttpClient _httpClient;

        public SpecialtySevice(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);

        }

        public async Task<IEnumerable<SpecialtyViewModel>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/v1/specialties");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<SpecialtyViewModel>>(response);
        }

        public async Task<IEnumerable<SpecialtyViewModel>> GetAllParentSpecialities()
        {
            var response = await _httpClient.GetAsync("/api/v1/specialties/get-all-parent-specialities");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<SpecialtyViewModel>>(response);
        }

        public async Task<SpecialtyViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/specialties/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<SpecialtyViewModel>(response);
        }

        public async Task<ResponseResult> Create(SpecialtyViewModel specialtyViewModel)
        {
            var specialtyContent = GetContent(specialtyViewModel);

            var response = await _httpClient.PostAsync("/api/v1/specialties", specialtyContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Update(SpecialtyViewModel specialtyViewModel)
        {
            var specialtyContent = GetContent(specialtyViewModel);

            var id = specialtyViewModel.Id;

            var response = await _httpClient.PutAsync($"/api/v1/specialties/{id}", specialtyContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/specialties/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<IEnumerable<ComboViewModel>> ComboParentSpecialties()
        {
            var response = await _httpClient.GetAsync("/api/v1/specialties/combo-parent-specialties");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<ComboViewModel>>(response);
        }

        public async Task<IEnumerable<ComboViewModel>> ComboSpecialties()
        {
            var response = await _httpClient.GetAsync("/api/v1/specialties/combo-specialties");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<ComboViewModel>>(response);
        }
    }
}
