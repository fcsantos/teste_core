using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IInquiryScheduleRepository : IRepository<InquirySchedule>
    {
        Task<IEnumerable<InquirySchedule>> GetAllByDoctorUserId();
        Task<IEnumerable<InquirySchedule>> GetAllByPatientUserId();
        Task<IEnumerable<InquirySchedule>> GetAllByPatientId(Guid id);
        Task<IEnumerable<InquirySchedule>> GetAllByPatientAnsweredOrNot(bool answered);
        Task<IEnumerable<InquirySchedule>> GetAllByAnsweredOrNot(bool answered);
        Task<IEnumerable<InquirySchedule>> GetAllInquiryScheduleIsMailNotSender();

    }
}