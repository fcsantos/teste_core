using Core.Business.Models;
using System;
using System.Threading.Tasks;

namespace Core.Business.Intefaces
{
    public interface IInquiryService : IDisposable
    {
        Task Create(Inquiry inquiry);
        Task Update(Inquiry inquiry); 
        Task Delete(Guid id);
        Task Delete(Inquiry inquiry);
    }
}
