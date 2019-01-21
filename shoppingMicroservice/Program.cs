using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace shoppingMicroservice
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("shopping", false, false, false, null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    Connect().Wait();

                    var body = ea.Body;
                    var data = Encoding.UTF8.GetString(body);
                    var product = JsonConvert.DeserializeObject<ShoppingModel>(data);

                    hubConnection.InvokeAsync("PushProduct",product);
                    Console.WriteLine($"Product Name: {product.Name} - Quantity: {product.Quantity}");
                };

                channel.BasicConsume("shopping", true, consumer);
                Console.WriteLine("[X] Press Any Key To Exit");
                Console.ReadLine();
            }
        }

        public static HubConnection hubConnection;

        public static async Task Connect()
        {
            hubConnection=new HubConnectionBuilder()
            .WithUrl("http://localhost:1923/productsign")
            .Build();

            await hubConnection.StartAsync();
        }
    }
}
