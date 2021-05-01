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
    public class PathologyService : Service, IPathologyService
    {
        private readonly HttpClient _httpClient;

        public PathologyService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);

        }

        public async Task<IEnumerable<PathologyViewModel>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/v1/pathologies");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<PathologyViewModel>>(response);
        }

        public async Task<IEnumerable<PathologyViewModel>> GetAllParentPathologies()
        {
            var response = await _httpClient.GetAsync("/api/v1/pathologies/get-all-parent-pathologies");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<PathologyViewModel>>(response);
        }

        public async Task<PathologyViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/pathologies/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<PathologyViewModel>(response);
        }

        public async Task<ResponseResult> Create(PathologyViewModel pathologyViewModel)
        {
            var pathologyContent = GetContent(pathologyViewModel);

            var response = await _httpClient.PostAsync("/api/v1/pathologies", pathologyContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Update(PathologyViewModel pathologyViewModel)
        {
            var pathologyContent = GetContent(pathologyViewModel);

            var id = pathologyViewModel.Id;

            var response = await _httpClient.PutAsync($"/api/v1/pathologies/{id}", pathologyContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/pathologies/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }


        public async Task<IEnumerable<ComboViewModel>> ComboPathologies()
        {
            var response = await _httpClient.GetAsync("/api/v1/pathologies/combo-pathologies");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<ComboViewModel>>(response);
        }

        public async Task<IEnumerable<ComboViewModel>> ComboParentPathologies()
        {
            var response = await _httpClient.GetAsync("/api/v1/pathologies/combo-parent-pathologies");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<ComboViewModel>>(response);
        }
    }
}
