using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.BusinessLogic.CommandRequests;
using Core.Database;
using Core.Database.Abstract;
using Core.Models.Enums;
using Core.Models.Settings;
using Ether.Outcomes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Core.BusinessLogic.CommandHandlers
{
    public class FinishOrderCommandHandler : IRequestHandler<FinishOrderCommandRequest, IOutcome>
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<OrderContext> _dbContextFactory;
        private readonly AppSettings _appSettings;

        public FinishOrderCommandHandler(ILogger<FinishOrderCommandHandler> logger,
            AppSettings appSettings,
            IDbContextFactory<OrderContext> dbContextFactory)
        {
            _logger = logger;
            _appSettings = appSettings;
            _dbContextFactory = dbContextFactory;
        }

        public Task<IOutcome> Handle(FinishOrderCommandRequest request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                using (var dbContext = _dbContextFactory.Create(_appSettings.ConnectionStrings.OrdersDb))
                {

                    try
                    {
                        var foundOrder = dbContext.Orders.FirstOrDefault(e => e.Id == request.OrderId);
                        if (foundOrder == null)
                        {
                            return Outcomes.Failure().WithMessage($"Заказ № {request.OrderId} не найден");
                        }

                        foundOrder.Status = StatusEnum.Finished;
                        foundOrder.FinishDateTime = DateTime.Now;

                        dbContext.SaveChanges();

                        return (IOutcome) Outcomes.Success();
                    }
                    catch (Exception ex)
                    {
                        var logMessage = $"При закрытии заказа № {request.OrderId} возникли ошибки";

                        _logger.LogError(ex, logMessage);
                        return Outcomes.Failure().WithMessage(logMessage);
                    }
                }
            }, cancellationToken);
        }
    }
}