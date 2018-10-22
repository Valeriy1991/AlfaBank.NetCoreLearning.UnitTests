using Ether.Outcomes;

namespace Core.BusinessLogic.Notifications
{
    public interface INotifier : INotifier<Notification>
    {
    }
    public interface INotifier<in TNotification>
    {
        IOutcome Send(TNotification notification);
    }
}