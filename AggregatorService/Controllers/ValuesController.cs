using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AggregatorService.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IHttpClientFactory _factory;

        public OrderDetailsController(IHttpClientFactory factory)
        {
            this._factory = factory;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            string orderServiceUrl = Environment.GetEnvironmentVariable("ORDER_SERVICE_URL");
            if (string.IsNullOrWhiteSpace(orderServiceUrl))
            {
                orderServiceUrl = "http://order-service";
            }

            var orderData = await GetData("", orderServiceUrl + "/orders/" + id);


            string userServiceUrl = Environment.GetEnvironmentVariable("USER_SERVICE_URL");
            if (string.IsNullOrWhiteSpace(userServiceUrl))
            {
                userServiceUrl = "http://user-service";
            }

            var userData = await GetData("", userServiceUrl + "/user/" + id);

            var dict = JObject.Parse(orderData);
            var orders = dict["orders"];

            return "{userDetails: " + userData + "," + "orders: " + orders + "}";
        }

        private async Task<string> GetData(string clientName, string requestRUrl)
        {
            var client = _factory.CreateClient(clientName);

            var requestMsg = new HttpRequestMessage(HttpMethod.Get, requestRUrl); //todo

            var responseMsg = await client.SendAsync(requestMsg);

            var data = await responseMsg.Content.ReadAsStringAsync();

            return data;
        }
    }
}
