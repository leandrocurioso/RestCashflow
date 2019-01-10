using System;
using System.Threading.Tasks;
using RestCashflowLibrary.Domain.Model.Entity;

namespace RestCashflowLibrary.Domain.Service
{
    public interface IFinancialEntryValidateService : IService
    {
        bool IsEntryInThePast(DateTime currentDt, DateTime now);
        bool IsDayAccountLimitReached(DayBalanceEntity dayBalanceEntity);
    }
}
