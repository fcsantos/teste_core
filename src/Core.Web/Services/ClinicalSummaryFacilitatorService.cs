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
    public class ClinicalSummaryFacilitatorService : Service, IClinicalSummaryFacilitatorService
    {
        private readonly HttpClient _httpClient;

        public ClinicalSummaryFacilitatorService(HttpClient httpClient, 
                                                 IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);

        }

        public async Task<IEnumerable<ClinicalSummaryFacilitatorViewModel>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/v1/clinical-sumary-facilitators");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<ClinicalSummaryFacilitatorViewModel>>(response);
        }

        public async Task<ClinicalSummaryFacilitatorViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/clinical-sumary-facilitators/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<ClinicalSummaryFacilitatorViewModel>(response);
        }

        public async Task<IEnumerable<ClinicalSummaryFacilitatorViewModel>> GetAllByPathologyId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/clinical-sumary-facilitators/get-all-by-pathologyid/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<ClinicalSummaryFacilitatorViewModel>>(response);
        }

        public async Task<ResponseResult> Create(ClinicalSummaryFacilitatorViewModel clinicalSummaryFacilitatorViewModel)
        {
            var clinicalSummaryFacilitatorContent = GetContent(clinicalSummaryFacilitatorViewModel);

            var response = await _httpClient.PostAsync("/api/v1/clinical-sumary-facilitators", clinicalSummaryFacilitatorContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Update(ClinicalSummaryFacilitatorViewModel clinicalSummaryFacilitatorViewModel)
        {
            var clinicalSummaryFacilitatorContent = GetContent(clinicalSummaryFacilitatorViewModel);

            var id = clinicalSummaryFacilitatorViewModel.Id;

            var response = await _httpClient.PutAsync($"/api/v1/clinical-sumary-facilitators/{id}", clinicalSummaryFacilitatorContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/clinical-sumary-facilitators/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<IEnumerable<ComboViewModel>> Combo(string type)
        {
            var response = await _httpClient.GetAsync($"/api/v1/clinical-sumary-facilitators/combo/{type}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<ComboViewModel>>(response);
        }
    }
}
