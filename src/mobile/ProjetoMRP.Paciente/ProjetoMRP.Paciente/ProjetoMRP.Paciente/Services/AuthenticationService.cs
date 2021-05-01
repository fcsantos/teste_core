using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using ProjetoMRP.Paciente.Extensions;
using ProjetoMRP.Paciente.ViewModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoMRP.Paciente.Services
{
    public class AuthenticationService : Services, IAuthenticationService
    {
        private readonly HttpClient _httpClient;

        private readonly IAspNetUser _user;
        private readonly Microsoft.AspNetCore.Authentication.IAuthenticationService _authenticationService;

        public AuthenticationService(HttpClient httpClient,
                                   IOptions<AppSettings> settings,
                                   Microsoft.AspNetCore.Authentication.IAuthenticationService authenticationService,
                                   IAspNetUser user)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri(settings.Value.APICoreUrl);
            _user = user;
            _authenticationService = authenticationService;
        }

        public async Task<UserResponseLogin> Login(UserLoginViewModel userLoginViewModel)
        {
            var loginContent = GetContent(userLoginViewModel);

            var response = await _httpClient.PostAsync("/api/v1/login", loginContent);

            if (!HandlingErrorsResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = await DeserializedObjectResponse<ResponseResult>(response)
                };
            }

            return await DeserializedObjectResponse<UserResponseLogin>(response);
        }

        public async Task AccomplishLogin(UserResponseLogin response)
        {
            var token = GetTokenFormatted(response.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", response.AccessToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                IsPersistent = true
            };

            await _authenticationService.SignInAsync(
                _user.GetHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public static JwtSecurityToken GetTokenFormatted(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }

        public Task<ResponseResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            throw new NotImplementedException();
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            throw new NotImplementedException();
        }
    }
}
