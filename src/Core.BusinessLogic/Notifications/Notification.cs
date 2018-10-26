namespace Core.BusinessLogic.Notifications
{
    public abstract class Notification
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
    }
}