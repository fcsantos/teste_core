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
    public class MessageService : Service, IMessageService
    {
        private readonly HttpClient _httpClient;

        public MessageService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);
        }

        public async Task<IEnumerable<MessageViewModel>> GetAllMessagesReplyByDoctorId()
        {
            var response = await _httpClient.GetAsync("/api/v1/messages/get-all-messages-reply-bydoctor");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<MessageViewModel>>(response);
        }

        public async Task<IEnumerable<MessageViewModel>> GetAllMessagesSentByDoctorId()
        {
            var response = await _httpClient.GetAsync("/api/v1/messages/get-all-messages-sent-bydoctor");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<MessageViewModel>>(response);
        }

        public async Task<IEnumerable<MessageViewModel>> GetAllMessagesByPacientId()
        {
            var response = await _httpClient.GetAsync("/api/v1/messages/get-all-messages-bypatient");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<MessageViewModel>>(response);
        }

        public async Task<IEnumerable<MessageViewModel>> GetAllMessagesByPacientId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/messages/get-all-messages-bypatientid/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<MessageViewModel>>(response);
        }

        public async Task<MessageViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/messages/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<MessageViewModel>(response);
        }

        public async Task<MessageViewModel> GetByIdWithReplyMessage(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/messages/get-by-id-with-replymessage/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<MessageViewModel>(response);
        }

        public async Task<MessageViewModel> GetByReplyMessageId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/messages/get-by-reply-message-id/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<MessageViewModel>(response);
        }

        public async Task<IEnumerable<MessageViewModel>> GetAllNotReadMessagesToPatient()
        {
            var response = await _httpClient.GetAsync("/api/v1/messages/get-all-not-read-messages-to-patient");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<MessageViewModel>>(response);
        }

        public async Task<IEnumerable<MessageViewModel>> GetAllNotReadMessagesToDoctor()
        {
            var response = await _httpClient.GetAsync("/api/v1/messages/get-all-not-read-messages-to-doctor");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<MessageViewModel>>(response);
        }

        public async Task<IEnumerable<MessageViewModel>> GetAllAwaitingResponseMessagesToPatient()
        {
            var response = await _httpClient.GetAsync("/api/v1/messages/get-all-awaiting-response-messages-to-patient");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandlingErrorsResponse(response);

            return await DeserializedObjectResponse<IEnumerable<MessageViewModel>>(response);
        }

        public async Task<ResponseResult> Create(MessageViewModel messageViewModel)
        {
            var messageContent = GetContent(messageViewModel);

            var response = await _httpClient.PostAsync("/api/v1/messages", messageContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Update(MessageViewModel messageViewModel)
        {
            var messageContent = GetContent(messageViewModel);

            var id = messageViewModel.Id;

            var response = await _httpClient.PutAsync($"/api/v1/messages/{id}", messageContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/messages/{id}");

            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
