
namespace Core.Web.Models
{
    public class NotificationViewModel
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public string LabelFormatted { get; set; }
        public string ActionUrl { get; set; }
        public string Icon { get; set; }
    }
}
