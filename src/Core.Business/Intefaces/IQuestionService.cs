using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business.Models;

namespace Core.Business.Intefaces
{
    public interface IQuestionService : IDisposable
    {
        Task Create(Question question);
        Task Update(Question question);
        Task Delete(Guid id);
        Task Delete(Question question);
    }
}
