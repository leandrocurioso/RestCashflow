using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestCashflowLibrary.Domain.Model.Entity;

namespace RestCashflowLibrary.Infrastructure.Repository
{
    public interface IFinancialEntryRepository : IRepository
    {
        Task<long> Create(FinancialEntryEntity entity);
        Task<bool> Update(FinancialEntryEntity entity);
        Task<IEnumerable<FinancialEntryEntity>> ListConciledBetweenDate(DateTime startDate, DateTime endDate);
    }
}
