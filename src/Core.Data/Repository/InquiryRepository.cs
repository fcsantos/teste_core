using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Data.Repository
{
    public class InquiryRepository : Repository<Inquiry>, IInquiryRepository
    { 
        public InquiryRepository(MyDbContext context) : base(context) { }

        public async Task<Inquiry> GetQuestionsById(Guid id)
        {            
            return await Db.Inquiries.Include(d => d.Questions).ThenInclude(q => q.AnswerOptions).Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> GetAllActiveCount()
        {
            return await Db.Inquiries.AsNoTracking().Where(p => p.IsActive.Value).CountAsync();
        }
    }
}
