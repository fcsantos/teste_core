using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Web.Services
{
    public interface IManageUserClaimsService
    {

        Task<IEnumerable<AllUsersViewModel>> GetAllUsers();
        Task<IEnumerable<UserClaimsViewModel>> GetClaimsByUser(Guid id);
        Task<IEnumerable<AppControllerViewModel>> GetAllControllers();
        Task<IEnumerable<AppActionViewModel>> GetActionsByController(Guid id);
        Task<IEnumerable<AppActionViewModel>> GetAllActions();
        Task<ResponseResult> CreateClaims(ControllerActionsViewModel controllerActions);
        Task<ResponseResult> Delete(int id);
    }
}
