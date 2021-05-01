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
    public class NoticeService : Service, INoticeService
    {
        private readonly HttpClient _httpClient;

        public NoticeService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);
        }

        public async Task<IEnumerable<NoticeViewModel>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/v1/notices");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<NoticeViewModel>>(response);
        }

        public async Task<NoticeViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/notices/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<NoticeViewModel>(response);
        }

        public async Task<ResponseResult> Create(NoticeViewModel notice)
        {
            notice.IsActive = true;
            var noticeContent = GetContent(notice);

            var response = await _httpClient.PostAsync("/api/v1/notices", noticeContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Update(NoticeViewModel notice)
        {
            var noticeContent = GetContent(notice);

            var id = notice.Id;

            var response = await _httpClient.PutAsync($"/api/v1/notices/{id}", noticeContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/notices/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<IEnumerable<UserNoticeViewModel>> GetAllCurrentNoticeBy()
        {
            var response = await _httpClient.GetAsync("/api/v1/notices/get-all-current-notice-by");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<UserNoticeViewModel>>(response);
        }
    }
}
