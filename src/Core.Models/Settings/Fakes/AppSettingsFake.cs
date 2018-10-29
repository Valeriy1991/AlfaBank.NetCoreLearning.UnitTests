using System.Diagnostics.CodeAnalysis;

namespace Core.Models.Settings.Fakes
{
    [ExcludeFromCodeCoverage]
    public static class AppSettingsFake
    {
        public static AppSettings Generate()
        {
            return new AppSettings()
            {
                ConnectionStrings = new ConnectionStringSettings()
                {
                    OrdersDb = "test-db"
                },
                Notification = new NotificationSettings()
                {
                    Sms = new NotificationSettings.SmsSettings()
                    {
                        From = "0000"
                    }
                }
            };
        }
    }
}