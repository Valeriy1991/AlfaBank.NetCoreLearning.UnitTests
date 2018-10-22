using System.Threading;
using System.Threading.Tasks;
using Core.BusinessLogic.CommandRequests;
using Core.BusinessLogic.Notifications;
using Core.Database;
using Core.Database.Commands;
using Core.Database.DbExecutors;
using Core.Models;
using Ether.Outcomes;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.BusinessLogic.CommandHandlers
{
    public class MakeTaxiOrderCommandHandler : IRequestHandler<MakeTaxiOrderCommandRequest, IOutcome>
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<OrderContext> _dbContextFactory;
        private readonly AppSettings _appSettings;
        private readonly INotifier _notifier;

        public MakeTaxiOrderCommandHandler(
            ILogger<MakeTaxiOrderCommandHandler> logger,
            IOptions<AppSettings> appSettingsOptions,
            IDbContextFactory<OrderContext> dbContextFactory,
            INotifier notifier)
        {
            _logger = logger;
            _appSettings = appSettingsOptions.Value;
            _dbContextFactory = dbContextFactory;
            _notifier = notifier;
        }

        public Task<IOutcome> Handle(MakeTaxiOrderCommandRequest request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                _logger.LogInformation($"Начато формирование заказа на такси для клиента {request.Phone}");

                // 1. Создать новый заказ со статусом "В обработке"
                var createOrderResult = CreateNewOrder(request);
                if (createOrderResult.Failure)
                {
                    return createOrderResult;
                }

                // 2. Отправить клиенту СМС с номером заказа
                var createdOrder = createOrderResult.Value;
                var sendResult = SendNotification(createdOrder);

                return sendResult;
            }, cancellationToken);
        }

        private IOutcome<Order> CreateNewOrder(MakeTaxiOrderCommandRequest request)
        {
            using (var dbContext = _dbContextFactory.Create(_appSettings.ConnectionStrings.OrdersDb))
            {
                var command = new CreateNewOrderCommand(dbContext);
                return command.Execute(new CreateNewOrderCommand.Context()
                {
                    Comments = request.Comments,
                    Phone = request.Phone,
                    From = request.From,
                    To = request.To,
                    When = request.When
                });
            }
        }

        private IOutcome SendNotification(Order createdOrder)
        {
            var sms = new SmsNotification()
            {
                From = _appSettings.Notification.Sms.From,
                To = createdOrder.Phone,
                Message = $"Заказ № {createdOrder.Id} принят. Ожидайте такси к {createdOrder.When}"
            };
            return _notifier.Send(sms);
        }
    }
}