using Ether.Outcomes;

namespace Core.BusinessLogic.Notifications
{
    public class SmsNotifier : INotifier
    {
        public IOutcome Send(Notification notification)
        {
            return Outcomes.Success();
        }
    }
}