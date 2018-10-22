namespace Core.Models
{
    public class NotificationSettings
    {
        public SmsSettings Sms { get; set; }

        public class SmsSettings
        {
            public string From { get; set; }
        }
    }
}