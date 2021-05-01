using Core.Web.Extensions;
using Core.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Web.Services
{
    public class AuthenticationService : Service, IAuthenticationService
    {
        private readonly HttpClient _httpClient;

        private readonly IAspNetUser _user;
        private readonly AppSettings _appSettings;
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
            _appSettings = settings.Value;
        }

        public async Task<UserResponseLogin> Login(UserLogin userLogin)
        {
            var loginContent = GetContent(userLogin);

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
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(_appSettings.ExpirationHours),
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

        public async Task Logout()
        {
            await _authenticationService.SignOutAsync(
                _user.GetHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                null);
        }

        public async Task<ResponseResult> ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            var forgotPasswordContent = GetContent(forgotPassword);

            var response = await _httpClient.PostAsync("/api/v1/forgot-my-password", forgotPasswordContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            var resetPasswordContent = GetContent(resetPassword);

            var response = await _httpClient.PostAsync("/api/v1/reset-password", resetPasswordContent);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!HandlingErrorsResponse(response)) return await DeserializedObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}