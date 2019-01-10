using System.Data;
using RabbitMQ.Client;
using RestCashflowLibrary.Domain.Business;
using RestCashflowLibrary.Domain.Service;
using RestCashflowLibrary.Infrastructure.Connection;
using RestCashflowLibrary.Infrastructure.Connection.MySql;
using RestCashflowLibrary.Infrastructure.Connection.RabbitMq;
using RestCashflowLibrary.Infrastructure.Consumer;
using RestCashflowLibrary.Infrastructure.Queue;
using RestCashflowLibrary.Infrastructure.Repository;
using SimpleInjector;

namespace RestCashflowLibrary
{
    public static class CompositionRoot
    {
        public static void Register(Container container)
        {
            try
            {
                RegisterFactory(container);
                RegisterGeneral(container);
                RegisterRepository(container);
                RegisterBusiness(container);
                RegisterService(container);
                RegisterQueue(container);
                RegisterConsumer(container);
            }
            catch
            {
                throw;
            }
        }

        static void RegisterFactory(Container container)
        {
            try
            {
                container.Register<ISqlConnectionFactory, MySqlConnectionFactory>(Lifestyle.Singleton);
                container.Register<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>(Lifestyle.Singleton);
            }
            catch
            {
                throw;
            }
        }

        static void RegisterGeneral(Container container)
        {
            try
            {
                container.Register<IDbConnection>(() => container.GetInstance<ISqlConnectionFactory>().Fabricate(), Lifestyle.Scoped);
                container.Register<IMySqlBuildStructure, MySqlBuildStructure>(Lifestyle.Scoped);
                container.Register<IConnection>(() => container.GetInstance<IRabbitMqConnectionFactory>().Fabricate(), Lifestyle.Singleton);
                container.Register<IModel>(() => container.GetInstance<IConnection>().CreateModel(), Lifestyle.Singleton);
            }
            catch
            {
                throw;
            }
        }

        static void RegisterRepository(Container container)
        {
            try
            {
                container.Register<IFinancialEntryRepository, FinancialEntryRepository>(Lifestyle.Scoped);
                container.Register<IDayBalanceRepository, DayBalanceRepository>(Lifestyle.Scoped);
            }
            catch
            {
                throw;
            }
        }

        static void RegisterBusiness(Container container)
        {
            try
            {
                container.Register<IFinancialEntryBusiness, FinancialEntryBusiness>(Lifestyle.Scoped);
                container.Register<IDayBalanceBusiness, DayBalanceBusiness>(Lifestyle.Scoped);
            }
            catch
            {
                throw;
            }
        }

        static void RegisterService(Container container)
        {
            try
            {
                container.Register<IFinancialEntryValidateService, FinancialEntryValidateService>(Lifestyle.Scoped);
            }
            catch
            {
                throw;
            }
        }

        static void RegisterQueue(Container container)
        {
            try
            {
                container.Register<IPaymentQueue, PaymentQueue>(Lifestyle.Singleton);
                container.Register<IReceiptQueue, ReceiptQueue>(Lifestyle.Singleton);
                container.Register<IConciliateQueue, ConciliateQueue>(Lifestyle.Singleton);
            }
            catch
            {
                throw;
            }
        }

        static void RegisterConsumer(Container container)
        {
            try
            {
                container.Register<IPaymentConsumer, PaymentConsumer>(Lifestyle.Singleton);
                container.Register<IReceiptConsumer, ReceiptConsumer>(Lifestyle.Singleton);
                container.Register<IConciliateConsumer, ConciliateConsumer>(Lifestyle.Singleton);
            }
            catch
            {
                throw;
            }
        }
    }
}
