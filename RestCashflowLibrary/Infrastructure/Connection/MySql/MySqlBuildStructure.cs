using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace RestCashflowLibrary.Infrastructure.Connection.MySql
{
    public class MySqlBuildStructure : IMySqlBuildStructure
    {
        readonly IDbConnection _connection;

        public MySqlBuildStructure(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task CreateTables()
        {
            try
            {
                var sql = @"CREATE TABLE IF NOT EXISTS . `day_balance` (
                              `id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
                              `date` date NOT NULL,
                              `total_entry` decimal(11,4) NOT NULL,
                              `total_out` decimal(11,4) NOT NULL,
                              `interest` decimal(11,4) NOT NULL,
                              PRIMARY KEY (`id`,`date`)
                            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";

                sql += @"CREATE TABLE IF NOT EXISTS `financial_entry` (
                          `id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
                          `entry_type` int(11) NOT NULL,
                          `description` varchar(1000) NOT NULL,
                          `destination_account` varchar(50) NOT NULL,
                          `destination_bank` int(11) NOT NULL,
                          `destination_cpf_cnpj` varchar(20) NOT NULL,
                          `account_type` int(11) NOT NULL,
                          `value` decimal(11,4) NOT NULL,
                          `charge` decimal(11,4) NOT NULL,
                          `entry_date` datetime NOT NULL,
                          `created_at` datetime NOT NULL,
                          `conciled` int(11) NOT NULL DEFAULT 0,
                          `conciled_at` datetime DEFAULT NULL,
                          PRIMARY KEY (`id`)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";
                await _connection.ExecuteAsync(sql);
            }
            catch
            {
                throw;
            }
        }
    }
}
