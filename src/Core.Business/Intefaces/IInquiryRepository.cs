using Core.Business.Models;
using System;
using System.Threading.Tasks;

namespace Core.Business.Intefaces
{
    public interface IInquiryRepository : IRepository<Inquiry>
    {
        Task<Inquiry> GetQuestionsById(Guid id);

        Task<int> GetAllActiveCount();
    }
} 
