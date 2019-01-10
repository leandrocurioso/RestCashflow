using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestCashflowLibrary.Domain.Model.DataTransferObject;
using RestCashflowLibrary.Domain.Model.Entity;

namespace RestCashflowLibrary.Domain.Business
{
    public interface IFinancialEntryBusiness : IBusiness
    {
        Task AddToQueue(FinancialEntryEntity financialEntryEntity);
        Task<long> Create(FinancialEntryEntity entity);
        Task<IEnumerable<CashflowDataTransferObject>> GetCashflow(DateTime startDate, int daysAhead);
        decimal CalculateDayPositionPercentage(decimal? yesterdayTootal, decimal currentDayTotal);
    }
}
