using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.BusinessLogic.CommandHandlers;
using Core.BusinessLogic.QueryRequests;
using Core.BusinessLogic.WebServices;
using Core.Models;
using Core.Models.Settings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.BusinessLogic.QueryHandlers
{
    /// <summary>
    /// Получение списка свободных водителей
    /// </summary>
    public class VacantDriversQueryHandler : IRequestHandler<VacantDriversQueryRequest, List<Driver>>
    {
        private readonly ILogger _logger;
        private readonly DriverRestService _driverRestService;

        public VacantDriversQueryHandler(
            ILogger<VacantDriversQueryHandler> logger,
            DriverRestService driverRestService)
        {
            _logger = logger;
            _driverRestService = driverRestService;
        }

        public async Task<List<Driver>> Handle(VacantDriversQueryRequest request, 
            CancellationToken cancellationToken)
        {
            _logger.LogDebug("Получение списка свободных водителей...");

            var vacantDrivers = await _driverRestService.GetVacantDriversAsync(request.CurrentDateTime);

            _logger.LogDebug($"Найдено свободных водителей: {vacantDrivers.Count}");
            return vacantDrivers;
        }
    }
}