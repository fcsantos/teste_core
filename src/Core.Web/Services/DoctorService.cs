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
    public class DoctorService : Service, IDoctorService
    {
        private readonly HttpClient _httpClient;

        public DoctorService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);
        }

        public async Task<IEnumerable<DoctorViewModel>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/v1/doctors");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<DoctorViewModel>>(response);
        }

        public async Task<DoctorViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/doctors/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            var doctorVM = await DeserializedObjectResponse<DoctorViewModel>(response);
            var specialtyResponse = await _httpClient.GetAsync($"/api/v1/specialties");
            doctorVM.SystemSpecialties = await DeserializedObjectResponse<List<SpecialtyViewModel>>(specialtyResponse);

            return doctorVM;
        }

        public async Task<ResponseResult> Create(DoctorViewModel doctorViewModel)
        {
            var doctorContent = GetContent(doctorViewModel);

            var response = await _httpClient.PostAsync("/api/v1/doctors", doctorContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Update(DoctorViewModel doctorViewModel)
        {
            var doctorContent = GetContent(doctorViewModel);

            var id = doctorViewModel.Id;

            var response = await _httpClient.PutAsync($"/api/v1/doctors/{id}", doctorContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/doctors/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> DeleteUser(string id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<DoctorViewModel> GetByUserId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/doctors/get-by-userid/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<DoctorViewModel>(response);
        }

        public async Task<IEnumerable<ComboViewModel>> Combo()
        {
            var response = await _httpClient.GetAsync("/api/v1/doctors/combo");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<ComboViewModel>>(response);
        }
    }
}
