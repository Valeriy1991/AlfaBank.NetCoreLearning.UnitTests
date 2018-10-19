using System.Threading.Tasks;
using Core.BusinessLogic.CommandRequests;
using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Taxi.Api.Service.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class TaxiController : Controller
    {
        private readonly IMediator _mediator;

        public TaxiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Оформление заказа такси
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("make-order")]
        public async Task<IActionResult> MakeOrder([FromBody]MakeOrderTaxiModel model)
        {
            var makeOrderResult = await _mediator.Send(new MakeTaxiOrderCommandRequest()
            {
                From = model.From,
                To = model.To,
                Comments = model.Comments,
                Phone = model.Phone,
                When = model.When,
            });
            return Ok(makeOrderResult);
        }
    }
}