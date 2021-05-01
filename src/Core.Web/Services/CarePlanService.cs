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
    public class CarePlanService : Service, ICarePlanService
    {
        private readonly HttpClient _httpClient;

        public CarePlanService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);
        }

        public async Task<IEnumerable<CarePlanViewModel>> GetAll(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/careplans/get-all/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<CarePlanViewModel>>(response);
        }

        public async Task<IEnumerable<CarePlanViewModel>> GetAllExceptDoctor(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/careplans/get-all-except-doctor/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<CarePlanViewModel>>(response);
        }

        public async Task<CarePlanViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/careplans/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<CarePlanViewModel>(response);
        }

        public async Task<IEnumerable<CarePlanViewModel>> GetAllCarePlansByPacientId()
        {
            var response = await _httpClient.GetAsync($"/api/v1/careplans/get-all-careplans-bypatient");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<CarePlanViewModel>>(response);
        }

        public async Task<ResponseResult> Create(CarePlanViewModel carePlanViewModel)
        {
            var carePlanContent = GetContent(carePlanViewModel);

            var response = await _httpClient.PostAsync("/api/v1/careplans", carePlanContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Update(CarePlanViewModel carePlanViewModel)
        {
            var carePlanContent = GetContent(carePlanViewModel);

            var id = carePlanViewModel.Id;

            var response = await _httpClient.PutAsync($"/api/v1/careplans/{id}", carePlanContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/careplans/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
