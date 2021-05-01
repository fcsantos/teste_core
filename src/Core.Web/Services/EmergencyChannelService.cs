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
    public class EmergencyChannelService : Service, IEmergencyChannelService
    {
        private readonly HttpClient _httpClient;

        public EmergencyChannelService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);
        }

        public async Task<IEnumerable<EmergencyChannelViewModel>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/v1/EmergencyChannels");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<EmergencyChannelViewModel>>(response);
        }

        public async Task<EmergencyChannelViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/EmergencyChannels/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<EmergencyChannelViewModel>(response);
        }

        public async Task<ResponseResult> Create(EmergencyChannelViewModel emergencyChannelViewModel)
        {
            var emergencyChannelContent = GetContent(emergencyChannelViewModel);

            var response = await _httpClient.PostAsync("/api/v1/EmergencyChannels", emergencyChannelContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Update(EmergencyChannelViewModel emergencyChannelViewModel)
        {
            var emergencyChannelContent = GetContent(emergencyChannelViewModel);

            var id = emergencyChannelViewModel.Id;

            var response = await _httpClient.PutAsync($"/api/v1/EmergencyChannels/{id}", emergencyChannelContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/EmergencyChannels/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
