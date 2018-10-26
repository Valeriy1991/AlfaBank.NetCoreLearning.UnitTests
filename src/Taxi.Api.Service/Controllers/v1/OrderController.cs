using System.Threading.Tasks;
using Core.BusinessLogic.CommandRequests;
using Core.Models;
using Core.Models.ApiModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Taxi.Api.Service.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/order")]
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Оформление заказа такси
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("make")]
        public async Task<IActionResult> Make([FromBody]MakeOrderTaxiModel model)
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