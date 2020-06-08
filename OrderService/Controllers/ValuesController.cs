using Microsoft.AspNetCore.Mvc;

namespace OrderService.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        // GET api/values/1
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "{\"orders\":[{\"orderId\":1,\"orderAmount\":250,\"orderDate\":\"14-Apr-2020\"},{\"orderId\":2,\"orderAmount\":450,\"orderDate\":\"15-Apr-2020\"}]}";
        }
    }
}
