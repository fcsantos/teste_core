using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoMRP.Paciente.Extensions
{
    public class AppSettings
    {
        public AppSettings()
        {
            APICoreUrl = "https://localhost:5001";
        }

        public string APICoreUrl { get; set; }
        public int ExpirationHours { get; set; }
        public string RoleDoctor { get; set; }
        public string RolePatient { get; set; }
        public string RoleAdmin { get; set; }
        public string DefaultPassword { get; set; }
    }
}
