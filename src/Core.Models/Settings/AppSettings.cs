namespace Core.Models.Settings
{
    public class AppSettings
    {
        public ConnectionStringSettings ConnectionStrings { get; set; }
        public NotificationSettings Notification { get; set; }
        public WebServicesSettings WebServices { get; set; }
    }
}