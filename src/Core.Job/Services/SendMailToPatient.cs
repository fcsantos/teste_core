using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.Job.Services
{
    public interface ISendMailToPatient
    {
        Task SendAccessToNewPatientAsync(string urlBase);
        Task NotificationOfNewMessageToPatientAsync(string urlBase);
        Task NotificationOfInquiryScheduleToPatient(string urlBase);
    }
    public class SendMailToPatient : ISendMailToPatient
    {
        public async Task SendAccessToNewPatientAsync(string urlBase)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(urlBase + "/api/v1/jarvis/send-access-new-patient");
                var resultContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Task failed.");
                }
            }
        }

        public async Task NotificationOfNewMessageToPatientAsync(string urlBase)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(urlBase + "/api/v1/jarvis/notification-new-message-patient");
                var resultContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Task failed.");
                }
            }
        }

        public async Task NotificationOfInquiryScheduleToPatient(string urlBase)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(urlBase + "/api/v1/jarvis/notification-inquiry-schedule-patient");
                var resultContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Task failed.");
                }
            }
        }
    }
}
