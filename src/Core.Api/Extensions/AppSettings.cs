using System.Collections.Generic;

namespace Core.Api.Extensions
{
    public class AppSettings
    {
        //Chave de criptografia do token
        public string Secret { get; set; }
        
        //quantas horas o token tem de validade
        public int ExpirationHours { get; set; }
        
        //quem emite: a app 
        public string Issuer { get; set; }

        //em quais urls o token é válido
        public string ValidOn { get; set; }

        public string UrlResetPassword { get; set; }

        public string UrlLogin { get; set; }

        public string Url { get; set; }

        public Dictionary<string, string[]> ClaimsListDoctor { get; set; }

        public Dictionary<string, string[]> ClaimsListPatient { get; set; }

        public string RoleDoctor { get; set; }
        public string RolePatient { get; set; }
        public string RoleAdmin { get; set; }

        public string DefaultPassword { get; set; }
    }
}