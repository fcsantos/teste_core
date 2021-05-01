using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Web.Models;

namespace Core.Web.Services
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceViewModel>> GetAll();
        Task<ServiceViewModel> GetById(Guid id);
        Task<ResponseResult> Create(ServiceViewModel serviceViewModel);
        Task<ResponseResult> Update(ServiceViewModel serviceViewModel);
        Task<ResponseResult> Delete(Guid id);
    }
}
