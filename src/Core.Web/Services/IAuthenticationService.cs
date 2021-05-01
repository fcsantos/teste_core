using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Web.Models;

namespace Core.Web.Services
{
    public interface IAuthenticationService
    {
        Task<UserResponseLogin> Login(UserLogin userLogin);

        Task AccomplishLogin(UserResponseLogin response);

        Task Logout();

        Task<ResponseResult> ForgotPassword(ForgotPasswordViewModel forgotPassword);

        Task<ResponseResult> ResetPassword(ResetPasswordViewModel resetPassword);
    }
}