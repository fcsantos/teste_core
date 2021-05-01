using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Web.Models;

namespace Core.Web.Services
{
    public interface IAccountService
    {
        Task<UserResponseLogin> Register(UserRegister userRegister);

        Task<UserResponseLogin> RegisterUser(UserRegister userRegister);

        IEnumerable<RoleViewModel> GetRoles();
    }
}
