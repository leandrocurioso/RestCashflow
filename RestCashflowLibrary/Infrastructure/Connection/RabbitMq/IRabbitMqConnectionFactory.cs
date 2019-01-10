using RabbitMQ.Client;

namespace RestCashflowLibrary.Infrastructure.Connection.RabbitMq
{
    public interface IRabbitMqConnectionFactory
    {
        IConnection Fabricate();
    }
}
