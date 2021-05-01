using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ProjetoMRP.Paciente.Extensions
{
    public interface IAspNetUser
    {
        string Name { get; }
        Guid GetUserId();
        string GetUserEmail();
        string GetUserToken();
        string GetUserRefreshToken();
        bool IsAuthenticated();
        bool IsInRole(string role);
        string GetUserRole();
        bool GetUserRole(string role);
        IEnumerable<Claim> GetClaims();
        HttpContext GetHttpContext();
        string GetNameOfUser();
    }
}
