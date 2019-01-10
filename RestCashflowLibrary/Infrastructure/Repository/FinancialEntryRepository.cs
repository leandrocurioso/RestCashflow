using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestCashflowLibrary.Domain.Model.Entity;
using Dapper;
using System.Collections.Generic;

namespace RestCashflowLibrary.Infrastructure.Repository
{
    public class FinancialEntryRepository : IFinancialEntryRepository
    {
        readonly IDbConnection _connection;
        public IConfiguration Configuration { get; set; }

        public FinancialEntryRepository(
            IDbConnection connection,
            IConfiguration configuration
        )
        {
            _connection = connection;
            Configuration = configuration;
        }

        public async Task<long> Create(FinancialEntryEntity entity)
        {
            try
            {
                var sql = @"INSERT INTO financial_entry 
                (
                    entry_type,
                    description,
                    destination_account,
                    destination_bank,
                    destination_cpf_cnpj,
                    account_type,
                    value,
                    charge,
                    entry_date,
                    created_at,
                    conciled
                )
                VALUES
                (
                    @EntryType,
                    @Description,
                    @DestinationAccount,
                    @DestinationBank,
                    @DestinationCpfCnpj,
                    @AccountType,
                    @Value,
                    @Charge,
                    @EntryDate,
                    @CreatedAt,
                    @Conciled
                );
                SELECT LAST_INSERT_ID();";
                return await _connection.ExecuteScalarAsync<long>(sql, new
                {
                    EntryType = entity.EntryType,
                    Description = entity.Description,
                    DestinationAccount = entity.DestinationAccount,
                    DestinationBank = entity.DestinationBank,
                    DestinationCpfCnpj = entity.DestinationCpfCnpj,
                    AccountType = entity.AccountType,
                    Value = entity.Value,
                    Charge = entity.Charge,
                    EntryDate = entity.EntryDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    CreatedAt = entity.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    Conciled = entity.Conciled
                });
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Update(FinancialEntryEntity entity)
        {
            try
            {
                var sql = @"UPDATE financial_entry SET
                                conciled =  @Conciled,
                                conciled_at = @ConciledAt
                            WHERE id = @Id;";
                var result = await _connection.ExecuteAsync(sql, new
                {
                    Id = entity.Id,
                    Conciled = entity.Conciled,
                    ConciledAt = entity.ConciledAt.ToString("yyyy-MM-dd HH:mm:ss")
                });
                return result == 1;
            }
            catch
            {
                throw;
            }
        }

        public Task<IEnumerable<FinancialEntryEntity>> ListConciledBetweenDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                var sql = @"SELECT  financial_entry.id, 
                                    financial_entry.entry_type, 
                                    financial_entry.description, 
                                    financial_entry.destination_account, 
                                    financial_entry.destination_bank, 
                                    financial_entry.destination_cpf_cnpj, 
                                    financial_entry.account_type, 
                                    financial_entry.`value`, 
                                    financial_entry.charge, 
                                    financial_entry.created_at, 
                                    financial_entry.entry_date, 
                                    financial_entry.conciled, 
                                    financial_entry.conciled_at
                            FROM financial_entry
                            WHERE financial_entry.conciled = 1
                            AND financial_entry.entry_date BETWEEN @StartDate AND @EndDate
                            ORDER BY financial_entry.entry_date ASC;";
                return  _connection.QueryAsync<FinancialEntryEntity>(sql, new
                {
                    StartDate = startDate.ToString("yyyy-MM-dd"),
                    EndDate = endDate.ToString("yyyy-MM-dd")
                });
            }
            catch
            {
                throw;
            }
        }
    }
}
