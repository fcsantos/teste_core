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
    public class ServiceService : Service, IServiceService
    {
        private readonly HttpClient _httpClient;

        public ServiceService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);

        }

        public async Task<IEnumerable<ServiceViewModel>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/v1/services");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<ServiceViewModel>>(response);
        }

        public async Task<ServiceViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/services/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<ServiceViewModel>(response);
        }

        public async Task<ResponseResult> Create(ServiceViewModel serviceViewModel)
        {
            var serviceContent = GetContent(serviceViewModel);

            var response = await _httpClient.PostAsync("/api/v1/services/createservice", serviceContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Update(ServiceViewModel service)
        {
            var serviceContent = GetContent(service);
            
            var id = service.Id;
            
            var response = await _httpClient.PutAsync($"/api/v1/services/{id}", serviceContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/services/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

    }
}
