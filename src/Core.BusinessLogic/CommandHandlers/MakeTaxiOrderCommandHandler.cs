using System.Threading;
using System.Threading.Tasks;
using Core.BusinessLogic.CommandRequests;
using Ether.Outcomes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.BusinessLogic.CommandHandlers
{
    public class MakeTaxiOrderCommandHandler : IRequestHandler<MakeTaxiOrderCommandRequest, IOutcome>
    {
        private readonly ILogger _logger;

        public MakeTaxiOrderCommandHandler(ILogger<MakeTaxiOrderCommandHandler> logger)
        {
            _logger = logger;
        }

        public Task<IOutcome> Handle(MakeTaxiOrderCommandRequest request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                _logger.LogInformation($"Начато формирование заказа на такси для клиента {request.Phone}");

                return (IOutcome) Outcomes.Success();
            }, cancellationToken);
        }
    }
}