using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestCashflowLibrary.Domain.Model.Entity;
using Dapper;
using System.Linq;

namespace RestCashflowLibrary.Infrastructure.Repository
{
    public class DayBalanceRepository : IDayBalanceRepository
    {
        readonly IDbConnection _connection;
        public IConfiguration Configuration { get; set; }

        public DayBalanceRepository(
            IDbConnection connection,
            IConfiguration configuration
        )
        {
            _connection = connection;
            Configuration = configuration;
        }

        public async Task<bool> Update(DayBalanceEntity entity)
        {
            try
            {
                var sql = @"UPDATE day_balance SET
                                total_entry = @TotalEntry,
                                total_out = @TotalOut,
                                interest = @Interest
                            WHERE date = @Date;";
                var result = await _connection.ExecuteAsync(sql, new
                {
                    TotalEntry = entity.TotalEntry,
                    TotalOut = entity.TotalOut,
                    Interest = entity.Interest,
                    Date = entity.Date.ToString("yyyy-MM-dd")
                });
                return result == 1;
            }
            catch
            {
                throw; 
            }
        }

        public async Task<long> Insert(DayBalanceEntity entity)
        {
            try
            {
                var sql = @"INSERT INTO day_balance 
                (
                    date,
                    total_entry,
                    total_out,
                    interest
                )
                VALUES
                (
                    @Date,
                    @TotalEntry,
                    @TotalOut,
                    @Interest
                );
                SELECT LAST_INSERT_ID();";
                return await _connection.ExecuteScalarAsync<long>(sql, new
                {
                    TotalEntry = entity.TotalEntry,
                    TotalOut = entity.TotalOut,
                    Interest = entity.Interest,
                    Date = entity.Date.ToString("yyyy-MM-dd")
                });
            }
            catch
            {
                throw;
            }
        }

        public async Task<DayBalanceEntity> GetByDate(DateTime dt)
        {
            try
            {
                var sql = @"SELECT day_balance.id, 
                                   day_balance.date, 
                                   day_balance.total_entry, 
                                   day_balance.total_out, 
                                   day_balance.interest
                            FROM day_balance
                            WHERE day_balance.date = @Date;";
                var result = await _connection.QueryAsync<DayBalanceEntity>(sql, new
                {
                    Date = dt.ToString("yyyy-MM-dd")
                });
                return result.FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }
    }
}
