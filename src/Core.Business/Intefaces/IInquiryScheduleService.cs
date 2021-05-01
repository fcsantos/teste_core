using System;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IInquiryScheduleService : IDisposable
    {
        Task Create(InquirySchedule inquirySchedule);
        Task CreateMany(InquirySchedule inquirySchedule);
        Task Update(InquirySchedule inquirySchedule);
        Task UpdateEmailSender(InquirySchedule inquirySchedule);
        Task Delete(Guid id);
        Task Delete(InquirySchedule inquirySchedule);
    } 
}