using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.Validations;

namespace Core.Business.Services
{
    public class NoticeService : BaseService, INoticeService
    {
        private readonly INoticeRepository _noticeRepository;
        private readonly INoticeUserRepository _noticeuserRepository;
        private readonly IUser _user;

        public NoticeService(INoticeRepository noticeRepository,
                           INoticeUserRepository noticeuserRepository,
                           INotifier notificador,
                           IUser user) : base(notificador)
        {
            _noticeRepository = noticeRepository;
            _noticeuserRepository = noticeuserRepository;
            _user = user;
        }

        public async Task Create(Notice notice)
        {
            if (!ExecuteValidation(new NoticeValidation(), notice)) return;            

            if (_noticeRepository.Search(f => f.Description == notice.Description && 
                                              f.StartDate == notice.StartDate && 
                                              f.EndDate == notice.EndDate).Result.Any())
            {
                Notification("Já existe um Aviso criado com estas informações.");
                return;
            }

            else if (notice.NoticeUsers.Count() == 0 && notice.SendToAllUsers == false)
            {
                Notification("Favor selecione um ou mais pacientes ao aviso.");
                return;
            }

            notice.Status = StatusNotice.Created;
            notice.EndDate = Convert.ToDateTime(notice.EndDate.ToShortDateString());         

            await _noticeRepository.Create(AuditColumns<Notice>(notice, "Create", _user.GetUserId()));
        }

        public async Task Update(Notice notice)
        {
            if (!ExecuteValidation(new NoticeValidation(), notice)) return;

            if (_noticeRepository.Search(f => f.Description == notice.Description &&
                                              f.StartDate == notice.StartDate &&
                                              f.EndDate == notice.EndDate &&
                                              f.Id != notice.Id).Result.Any())
            {
                Notification("Já existe um Aviso criado com estas informações.");
                return;
            }

            else if (notice.NoticeUsers.Count() == 0 && notice.SendToAllUsers == false)
            {
                Notification("Favor selecione um ou mais pacientes ao aviso.");
                return;
            }

            notice.Status = StatusNotice.Modified;
            notice.EndDate = Convert.ToDateTime(notice.EndDate.ToShortDateString());

            await UpdateNoticeUsers(notice);

            await _noticeRepository.Update(AuditColumns<Notice>(notice, "Update", _user.GetUserId()));
        }

        public async Task UpdateNoticeUsers(Notice notice)
        {
            var savedNoticeUsers = _noticeuserRepository.GetByNoticeId(notice.Id).Result;

            foreach (var noticeUser in savedNoticeUsers)
                await _noticeuserRepository.Delete(noticeUser);
        }

        public async Task Delete(Guid id)
        {
            await _noticeRepository.Delete(id);
        }

        public async Task Delete(Notice notice)
        {
            notice.IsActive = notice.IsActive.Value ? false : true;
            await _noticeRepository.Update(AuditColumns<Notice>(notice, "Update", _user.GetUserId()));
        }

        public void Dispose()
        {
            _noticeRepository?.Dispose();
            _noticeuserRepository?.Dispose();
        }


    }
}