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
    public interface IFornecedorService
    {
        Task<IEnumerable<FornecedorViewModel>> GetAll();

        Task<FornecedorViewModel> GetById(Guid id);

        Task<ResponseResult> Create(FornecedorViewModel fornecedorViewModel);

        Task<ResponseResult> Update(FornecedorViewModel fornecedorViewModel);

        Task<ResponseResult> Delete(Guid id);
    }

    public class FornecedorService : Service, IFornecedorService
    {
        private readonly HttpClient _httpClient;

        public FornecedorService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);
        }

        public async Task<IEnumerable<FornecedorViewModel>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/v1/fornecedores");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<FornecedorViewModel>>(response);
        }

        public async Task<FornecedorViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/fornecedores/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<FornecedorViewModel>(response);
        }

        public async Task<ResponseResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            var fornecedorContent = GetContent(fornecedorViewModel);

            var response = await _httpClient.PostAsync("/api/v1/fornecedores", fornecedorContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Update(FornecedorViewModel fornecedorViewModel)
        {
            var fornecedorContent = GetContent(fornecedorViewModel);

            var id = fornecedorViewModel.Id;

            var response = await _httpClient.PutAsync($"/api/v1/fornecedores/{id}", fornecedorContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }


        public async Task<ResponseResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/fornecedores/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
