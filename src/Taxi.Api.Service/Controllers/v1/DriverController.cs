using System.Threading.Tasks;
using Core.BusinessLogic.CommandRequests;
using Core.BusinessLogic.QueryRequests;
using Core.Models.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Taxi.Api.Service.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/driver")]
    public class DriverController : Controller
    {
        private readonly IMediator _mediator;

        public DriverController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Получение списка водителей, свободных на текущую дату и время
        /// </summary>
        /// <returns></returns>
        [HttpGet("vacant")]
        public async Task<IActionResult> GetVacantDrivers()
        {
            var vacantDrivers = await _mediator.Send(new VacantDriversQueryRequest());
            return Ok(vacantDrivers);
        }
    }
}