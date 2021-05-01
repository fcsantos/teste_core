using System;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class InquiryScheduleService : BaseService, IInquiryScheduleService
    {
        private readonly IInquiryScheduleRepository _inquiryScheduleRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUser _user;

        public InquiryScheduleService(IInquiryScheduleRepository inquiryScheduleRepository,
                                      IDoctorRepository doctorRepository,
                                      INotifier notifier, IUser user) : base(notifier)
        {
            _inquiryScheduleRepository = inquiryScheduleRepository;
            _doctorRepository = doctorRepository;
            _user = user;
        }

        public async Task Create(InquirySchedule inquirySchedule)
        {
            if (!ExecuteValidation(new InquiryScheduleValidation(), inquirySchedule)) return;

            inquirySchedule.DoctorId = _doctorRepository.GetDoctorByUserId(_user.GetUserId().ToString()).Result.Id;

            await _inquiryScheduleRepository.Create(AuditColumns<InquirySchedule>(inquirySchedule, "Create", _user.GetUserId()));
        }

        public async Task CreateMany(InquirySchedule inquirySchedule)
        {
            var dateInitial = inquirySchedule.StartDate;
            var dateDiff = inquirySchedule.EndDate.Subtract(inquirySchedule.StartDate).TotalDays;

            for (int i = 0; i <= dateDiff; i++)
            {
                inquirySchedule.StartDate = dateInitial.AddDays(i);
                inquirySchedule.EndDate = inquirySchedule.StartDate;
                inquirySchedule.Id = new Guid();
                await Create(inquirySchedule);
            }
        }

        public async Task Update(InquirySchedule inquirySchedule)
        {
            //if (!ExecuteValidation(new InquiryScheduleValidation(), inquirySchedule)) return;
            await _inquiryScheduleRepository.Update(AuditColumns<InquirySchedule>(inquirySchedule, "Update", _user.GetUserId()));
        }

        public async Task UpdateEmailSender(InquirySchedule inquirySchedule)
        {
            inquirySchedule.IsMailSender = true;
            await _inquiryScheduleRepository.Update(AuditColumns<InquirySchedule>(inquirySchedule, "Update", _user.GetUserId()));
        }

        public async Task Delete(Guid id)
        {
            await _inquiryScheduleRepository.Delete(id);
        }

        public async Task Delete(InquirySchedule inquirySchedule)
        {
            //inquirySchedule.IsActive = inquirySchedule.IsActive.Value ? false : true;
            await _inquiryScheduleRepository.Update(AuditColumns<InquirySchedule>(inquirySchedule, "Update", _user.GetUserId()));
        }

        public void Dispose()
        {
            _inquiryScheduleRepository?.Dispose();
            _doctorRepository?.Dispose();
        }
    }
}
