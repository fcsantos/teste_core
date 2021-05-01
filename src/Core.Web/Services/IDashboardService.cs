using System.Threading.Tasks;
using System.Collections.Generic;
using Core.Web.Models;

namespace Core.Web.Services
{
    public interface IDashboardService
    {
        Task<int?> GetAllActiveInquiries();
        Task<int?> GetAllActivePatients();
        Task<int?> GetAllConsultantByDoctorId();
        Task<int?> GetAllDiagnosticByDoctorId();
        Task<IEnumerable<InquiryScheduleViewModel>> GetAllInquiryAnswered();
        Task<IEnumerable<MessageViewModel>> GetAllAnsweredMessages();
    }
}
