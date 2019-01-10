using System;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace RestCashflowLibrary.Infrastructure.Connection.RabbitMq
{
    public class RabbitMqConnectionFactory : IRabbitMqConnectionFactory
    {
        readonly IConfiguration _configuration;
        
        public RabbitMqConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConnection Fabricate()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("RabbitMqDefault");
                var factory = new ConnectionFactory() {
                   Uri = new Uri(connectionString)
                };
                return factory.CreateConnection();
            }
            catch
            {
                throw;
            }
        }
    }
}
