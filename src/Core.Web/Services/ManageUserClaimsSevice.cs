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
    public class ManageUserClaimsSevice : Service, IManageUserClaimsService
    {
        private readonly HttpClient _httpClient;

        public ManageUserClaimsSevice(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);

        }

        public async Task<IEnumerable<AllUsersViewModel>> GetAllUsers()
        {
            var response = _httpClient.GetAsync("/api/v1/manageuserclaims/get-all-users").Result;
            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<AllUsersViewModel>>(response);
        }


        public async Task<IEnumerable<UserClaimsViewModel>> GetClaimsByUser(Guid id)
        {
            var response = _httpClient.GetAsync($"/api/v1/manageuserclaims/get-claims-byuser/{id}").Result;
            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<UserClaimsViewModel>>(response);
        }

        public async Task<IEnumerable<AppControllerViewModel>> GetAllControllers()
        {
            var response = await _httpClient.GetAsync("/api/v1/manageuserclaims/get-all-controllers");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<AppControllerViewModel>>(response);
        }


        public async Task<IEnumerable<AppActionViewModel>> GetAllActions()
        {
            var response = await _httpClient.GetAsync("/api/v1/manageuserclaims/get-all-actions");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<AppActionViewModel>>(response);
        }

        public async Task<IEnumerable<AppActionViewModel>> GetActionsByController(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/manageuserclaims/get-actions-bycontroller/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<AppActionViewModel>>(response);
        }

        public async Task<ResponseResult> CreateClaims(ControllerActionsViewModel controllerActionsViewModel)
        {
            var controllerContent = GetContent(controllerActionsViewModel);

            var response = await _httpClient.PostAsync("/api/v1/manageuserclaims", controllerContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/manageuserclaims/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
