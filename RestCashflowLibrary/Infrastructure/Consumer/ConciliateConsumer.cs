using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RestCashflowLibrary.Domain.Business;
using RestCashflowLibrary.Domain.Model.Entity;
using RestCashflowLibrary.Infrastructure.Repository;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace RestCashflowLibrary.Infrastructure.Consumer
{
    public class ConciliateConsumer : IConciliateConsumer
    {
        readonly Container _container;

        public ConciliateConsumer(Container container)
        {
            _container = container;
        }

        private async Task Execute(FinancialEntryEntity entity)
        {
            try
            {
                var financialEntryRepository = _container.GetInstance<IFinancialEntryRepository>();
                var dayBalanceBusiness = _container.GetInstance<IDayBalanceBusiness>();
                var dayBalanceRepository = _container.GetInstance<IDayBalanceRepository>();

                entity.Conciled = true;
                entity.ConciledAt = DateTime.Now;
                await financialEntryRepository.Update(entity);

                var dayBalance = await dayBalanceRepository.GetByDate(entity.EntryDate);
                if (dayBalance == null)
                {
                    await dayBalanceBusiness.Insert(entity);
                }
                else
                {
                    await dayBalanceBusiness.Update(entity, dayBalance);
                }
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

                channel.QueueDeclare(queue: "conciliate",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    using (AsyncScopedLifestyle.BeginScope(_container))
                    {
                        var json = Encoding.UTF8.GetString(ea.Body);
                        var entity = JsonConvert.DeserializeObject<FinancialEntryEntity>(json);
                        await Execute(entity);
                    }
                };
                channel.BasicConsume(queue: "conciliate",
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
