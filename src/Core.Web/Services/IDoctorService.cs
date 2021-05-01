using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Web.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorViewModel>> GetAll();

        Task<DoctorViewModel> GetById(Guid id);

        Task<DoctorViewModel> GetByUserId(Guid id);

        Task<ResponseResult> Create(DoctorViewModel patient);

        Task<ResponseResult> Update(DoctorViewModel patient);

        Task<ResponseResult> Delete(Guid id);

        Task<ResponseResult> DeleteUser(string id);

        Task<IEnumerable<ComboViewModel>> Combo();
    }
}
