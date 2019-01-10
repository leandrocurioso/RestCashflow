using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SimpleInjector;

namespace RestCashflowLibrary.Infrastructure.Queue
{
    public class ReceiptQueue : IReceiptQueue
    {
        readonly Container _container;

        public ReceiptQueue(Container container)
        {
            _container = container;

        }

        public Task Publish(object data)
        {
            try
            {
                var channel = _container.GetInstance<IModel>();
                channel.QueueDeclare(queue: "receipt",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                var json = JsonConvert.SerializeObject(data);
                channel.BasicPublish(exchange: "",
                                     routingKey: "receipt",
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
