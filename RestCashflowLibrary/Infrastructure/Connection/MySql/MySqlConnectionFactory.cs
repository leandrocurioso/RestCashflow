using System.Data;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace RestCashflowLibrary.Infrastructure.Connection.MySql
{
    public class MySqlConnectionFactory : ISqlConnectionFactory
    {
        readonly IConfiguration _configuration;

        static MySqlConnectionFactory(){
           Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;  
        }

        public MySqlConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection Fabricate()
        {
            try {
                var connectionString = _configuration.GetConnectionString("MySqlDefault");
                return new MySqlConnection(connectionString);
            } catch {
                throw;
            }
        }
    }
}
