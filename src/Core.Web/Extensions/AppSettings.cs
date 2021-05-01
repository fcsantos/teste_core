namespace Core.Web.Extensions
{
    public class AppSettings
    {
        public string APICoreUrl { get; set; }
        public int ExpirationHours { get; set; }
        public string RoleDoctor { get; set; }
        public string RolePatient { get; set; }
        public string RoleAdmin { get; set; }
        public string DefaultPassword { get; set; }
    }
}
 