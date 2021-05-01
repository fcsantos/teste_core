using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Business.Services
{
    public class InquiryService : BaseService, IInquiryService
    {
        private readonly IInquiryRepository _inquiryRepository;
        private readonly IUser _user;
         
        public InquiryService(IInquiryRepository inquiryRepository,
                                IQuestionRepository questionRepository,
                                INotifier notifier,
                                IUser user) : base(notifier)
        {
            _inquiryRepository = inquiryRepository;
            _user = user;
        }

        public async Task Create(Inquiry inquiry)
        {
            if (!ExecuteValidation(new InquiryValidation(), inquiry)) return;

            if (_inquiryRepository.Search(s => s.Title == inquiry.Title).Result.Any())
            {
                Notification("Já existe uma formulário com este nome.");
                return;
            }

            await _inquiryRepository.Create(AuditColumns<Inquiry,Question>(inquiry, "Create", _user.GetUserId()));
        }

        public async Task Update(Inquiry inquiry)
        {
            if (!ExecuteValidation(new InquiryValidation(), inquiry)) return;

            if (_inquiryRepository.Search(s => s.Title == inquiry.Title && s.Id != inquiry.Id).Result.Any())
            {
                Notification("Já existe uma formulário com este nome.");
                return;
            }

            await _inquiryRepository.Update(AuditColumns<Inquiry>(inquiry, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            await _inquiryRepository.Delete(id);
        }

        public async Task Delete(Inquiry inquiry)
        {
            inquiry.IsActive = inquiry.IsActive.Value ? false : true;
            await _inquiryRepository.Update(AuditColumns<Inquiry>(inquiry, "Update", _user.GetUserId()));
        }

        public void Dispose()
        {
            _inquiryRepository?.Dispose();
        }
    }
}
