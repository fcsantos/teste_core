namespace Core.Api.Extensions
{
    public class MailSettings
    {
        public string Smtp { get; set; } 
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TemplateRegistro { get; set; }
        public string TemplateRedefinir { get; set; }
        public string TemplateNotification { get; set; }
        public string DisplayName { get; set; }
        public string From { get; set; }
        public string Name { get; set; }
    }
}