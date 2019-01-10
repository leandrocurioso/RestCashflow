using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SimpleInjector;

namespace RestCashflowLibrary.Infrastructure.Queue
{
    public class PaymentQueue : IPaymentQueue
    {
        readonly Container _container;

        public PaymentQueue(Container container)
        {
            _container = container;
        }

        public Task Publish(object data)
        {
            try
            {
                var channel = _container.GetInstance<IModel>();
                channel.QueueDeclare(queue: "payment",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                var json = JsonConvert.SerializeObject(data);
                channel.BasicPublish(exchange: "",
                                     routingKey: "payment",
                                     basicProperties: null,
                                     body: Encoding.UTF8.GetBytes(json));
                return Task.CompletedTask;
            }
            catch
            {
                throw;
            }
        }
    }
}
