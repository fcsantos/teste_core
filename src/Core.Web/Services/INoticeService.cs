using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Web.Models;

namespace Core.Web.Services
{
    public interface INoticeService
    {
        Task<IEnumerable<NoticeViewModel>> GetAll();

        Task<NoticeViewModel> GetById(Guid id);

        Task<ResponseResult> Create(NoticeViewModel notice);

        Task<ResponseResult> Update(NoticeViewModel notice);

        Task<ResponseResult> Delete(Guid id);

        Task<IEnumerable<UserNoticeViewModel>> GetAllCurrentNoticeBy();
    }
}
