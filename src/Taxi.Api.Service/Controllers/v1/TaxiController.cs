using Microsoft.AspNetCore.Mvc;

namespace Taxi.Api.Service.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class TaxiController : Controller
    {
        [HttpPost("make-order")]
        public IActionResult MakeOrder([FromBody]MakeOrderTaxiModel model)
        {
            return
            View();
        }
    }
}