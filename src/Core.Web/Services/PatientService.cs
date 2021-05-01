using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Core.Web.Extensions;
using Core.Web.Models;
using Microsoft.Extensions.Options;

namespace Core.Web.Services
{
    public class PatientService : Service, IPatientService
    {
        private readonly HttpClient _httpClient;

        public PatientService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);
        }

        public async Task<IEnumerable<PatientViewModel>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/v1/patients");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<PatientViewModel>>(response);
        }

        public async Task<PatientViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/patients/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<PatientViewModel>(response);
        }

        public async Task<ResponseResult> Create(PatientViewModel patientViewModel)
        {
            var patientContent = GetContent(patientViewModel);

            var response = await _httpClient.PostAsync("/api/v1/patients", patientContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Update(PatientViewModel patientViewModel)
        {
            var patientContent = GetContent(patientViewModel);

            var id = patientViewModel.Id;

            var response = await _httpClient.PutAsync($"/api/v1/patients/{id}", patientContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/patients/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> DeleteUser(string id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<PatientViewModel> GetByUserId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/patients/get-by-userid/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<PatientViewModel>(response);
        }


        public async Task<PagedViewModel<PatientViewModel>> GetAllPatientsPaged(int pageSize, int pageIndex, string query = null)
        {
            var response = await _httpClient.GetAsync($"/api/v1/patients/get-all-paged?ps={pageSize}&page={pageIndex}&q={query}");

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<PagedViewModel<PatientViewModel>>(response);
        }

        public async Task<IEnumerable<ComboViewModel>> Combo()
        {
            var response = await _httpClient.GetAsync("/api/v1/patients/combo");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<ComboViewModel>>(response);
        }
    }
}
