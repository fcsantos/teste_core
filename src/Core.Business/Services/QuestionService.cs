using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Business.Services
{
    public class QuestionService : BaseService, IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IUser _user;

        public QuestionService(IQuestionRepository questionRepository,
                                INotifier notifier,
                                IUser user) : base(notifier)
        {
            _questionRepository = questionRepository;
            _user = user;
        }

        public async Task Create(Question question)
        {
            if (!ExecuteValidation(new QuestionValidation(), question)) return;

            if (_questionRepository.Search(s => s.Title == question.Title).Result.Any())
            {
                Notification("Já existe uma pergunta com este título.");
                return;
            }

            await _questionRepository.Create(AuditColumns<Question>(question, "Create", _user.GetUserId()));

        }

        public async Task Update(Question question)
        {
            if (!ExecuteValidation(new QuestionValidation(), question)) return;

            if (_questionRepository.Search(s => s.Title == question.Title && s.Id != question.Id).Result.Any())
            {
                Notification("Já existe uma pergunta com este título.");
                return;
            }

            await _questionRepository.Update(AuditColumns<Question>(question, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            await _questionRepository.Delete(id);
        }

        public async Task Delete(Question question)
        {
            await _questionRepository.Update(AuditColumns<Question>(question, "Update", _user.GetUserId()));
        }

        public void Dispose()
        {
            _questionRepository?.Dispose();
        }
    }
}
