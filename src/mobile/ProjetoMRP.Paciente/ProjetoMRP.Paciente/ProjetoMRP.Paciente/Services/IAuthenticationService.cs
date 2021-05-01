using ProjetoMRP.Paciente.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoMRP.Paciente.Services
{
    public interface IAuthenticationService
    {
        Task<UserResponseLogin> Login(UserLoginViewModel userLoginViewModel);

        Task AccomplishLogin(UserResponseLogin response);

        Task Logout();

        Task<ResponseResult> ForgotPassword(ForgotPasswordViewModel forgotPassword);

        Task<ResponseResult> ResetPassword(ResetPasswordViewModel resetPassword);
    }
}
