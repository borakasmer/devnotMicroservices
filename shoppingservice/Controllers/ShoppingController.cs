using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace shoppingservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] ShoppingModel model)
        {
            var data = JsonConvert.SerializeObject(model);
            var body = Encoding.UTF8.GetBytes(data);

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("shopping", false, false, false, null);

                channel.BasicPublish("", "shopping", null, body);
            }
        }
    }

    public class ProductSign : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("GetConnectionID", Context.ConnectionId);
        }

        public async Task PushProduct(ShoppingModel product)
        {
            await Clients.All.SendAsync("GetProduct", product);
        }
    }

}
