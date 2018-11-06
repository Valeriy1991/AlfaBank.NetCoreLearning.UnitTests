using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.BusinessLogic.CommandRequests;
using Core.Database;
using Core.Database.Abstract;
using Core.Models;
using Core.Models.Settings;
using DbConn.DbExecutor.Abstract;
using Ether.Outcomes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.BusinessLogic.CommandHandlers
{
    public class SetDriverForOrderCommandHandler : IRequestHandler<SetDriverForOrderCommandRequest, IOutcome>
    {
        private readonly ILogger _logger;
        private readonly AppSettings _appSettings;
        private readonly IDbExecutorFactory _dbExecutorFactory;

        public SetDriverForOrderCommandHandler(
            ILogger<MakeOrderCommandHandler> logger,
            AppSettings appSettings,
            IDbExecutorFactory dbExecutorFactory)
        {
            _logger = logger;
            _appSettings = appSettings;
            _dbExecutorFactory = dbExecutorFactory;
        }

        public Task<IOutcome> Handle(SetDriverForOrderCommandRequest request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                try
                {
                    var driver = GetDriver(request.DriverId);
                    if (driver == null)
                    {
                        throw new NullReferenceException($"Назначаемый водитель (ID = {request.DriverId}) не найден");
                    }

                    SetDriverForOrder(request.OrderId, driver.Id);

                    var successMessage = $"На заказ № {request.OrderId} назначен водитель: {driver.FullName}";
                    _logger.LogInformation(
                        $"{successMessage} (ID = {driver.Id})");
                    return (IOutcome) Outcomes.Success().WithMessage(successMessage);
                }
                catch (Exception ex)
                {
                    var errorBaseMessage = $"$При назначении водителя на заказ № {request.OrderId} возникла ошибка";

                    var errorBuilder = new StringBuilder();
                    errorBuilder.Append($"{errorBaseMessage}: ");
                    errorBuilder.AppendLine();
                    errorBuilder.Append(ex.Message);

                    _logger.LogError(errorBuilder.ToString());
                    return Outcomes.Failure().WithMessage(errorBaseMessage);
                }
            }, cancellationToken);
        }

        private void SetDriverForOrder(int orderId, int driverId)
        {
            using (var dbExecutor = _dbExecutorFactory.Create(_appSettings.ConnectionStrings.OrdersDb))
            {
                var sql = $@"
insert into DriverOrders(DriverId, OrderId)
values ({driverId}, {orderId});
";
                dbExecutor.Execute(sql);
            }
        }

        private Driver GetDriver(int driverId)
        {
            using (var dbExecutor = _dbExecutorFactory.Create(_appSettings.ConnectionStrings.OrdersDb))
            {
                var sql = $@"
select * 
from Drivers
where Id = {driverId};
";
                return dbExecutor.Query<Driver>(sql).FirstOrDefault();
            }
        }
    }
}