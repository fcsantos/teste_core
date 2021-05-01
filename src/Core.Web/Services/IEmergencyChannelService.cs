using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Web.Services
{
    public interface IEmergencyChannelService
    {
        Task<IEnumerable<EmergencyChannelViewModel>> GetAll();

        Task<EmergencyChannelViewModel> GetById(Guid id);

        Task<ResponseResult> Create(EmergencyChannelViewModel emergencyChannelViewModel);

        Task<ResponseResult> Update(EmergencyChannelViewModel emergencyChannelViewModel);

        Task<ResponseResult> Delete(Guid id);
    }
}
