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
        public async Task<IActionResult> Make([FromBody]MakeOrderModel model)
        {
            var makeOrderResult = await _mediator.Send(new MakeOrderCommandRequest()
            {
                From = model.From,
                To = model.To,
                Comments = model.Comments,
                Phone = model.Phone,
                When = model.When,
            });
            return Ok(makeOrderResult);
        }

        /// <summary>
        /// Закрытие заказа
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost("finish/{orderId:int}")]
        public async Task<IActionResult> Finish(int orderId)
        {
            var finishOrderResult = await _mediator.Send(new FinishOrderCommandRequest()
            {
                OrderId = orderId
            });
            return Ok(finishOrderResult);
        }

        /// <summary>
        /// Назначить водителя на заказ
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="driverId"></param>
        /// <returns></returns>
        [HttpPost("{orderId:int}/set-driver/{driverId:int}")]
        public async Task<IActionResult> SetDriver(int orderId, int driverId)
        {
            var setDriverForOrderResult = await _mediator.Send(new SetDriverForOrderCommandRequest()
            {
                OrderId = orderId,
                DriverId = driverId
            });
            return Ok(setDriverForOrderResult);
        }


    }
}