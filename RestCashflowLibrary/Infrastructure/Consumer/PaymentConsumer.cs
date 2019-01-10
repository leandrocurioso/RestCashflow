using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RestCashflowLibrary.Domain.Model.Entity;
using SimpleInjector.Lifestyles;
using SimpleInjector;
using RestCashflowLibrary.Infrastructure.Queue;
using RestCashflowLibrary.Domain.Business;

namespace RestCashflowLibrary.Infrastructure.Consumer
{
    public class PaymentConsumer : IPaymentConsumer
    {
        readonly Container _container;

        public PaymentConsumer(Container container)
        {
            _container = container;
        }

        private async Task Execute(FinancialEntryEntity entity)
        {
            try
            {
                var financialEntryBusiness = _container.GetInstance<IFinancialEntryBusiness>();
                var conciliateQueue = _container.GetInstance<IConciliateQueue>();
                entity.Id = await financialEntryBusiness.Create(entity);
                await conciliateQueue.Publish(entity);
            }
            catch
            {
                throw;
            }
        }

        public void Consume()
        {
            try
            {
                var channel = _container.GetInstance<IModel>();

                channel.QueueDeclare(queue: "payment",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                                     
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    using (AsyncScopedLifestyle.BeginScope(_container))
                    {
                        var json = Encoding.UTF8.GetString(ea.Body);
                        var entity = JsonConvert.DeserializeObject<FinancialEntryEntity>(json);
                        Execute(entity).GetAwaiter().GetResult();
                    }
                };
                channel.BasicConsume(queue: "payment",
                                     exclusive: false,
                                     autoAck: true,
                                     consumer: consumer);
            }
            catch
            {
                throw;
            }
        }
    }
}
