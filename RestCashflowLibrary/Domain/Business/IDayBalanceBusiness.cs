using System.Threading.Tasks;
using RestCashflowLibrary.Domain.Model.Entity;

namespace RestCashflowLibrary.Domain.Business
{
    public interface IDayBalanceBusiness : IBusiness
    {
        Task<long> Insert(FinancialEntryEntity entity);
        Task<bool> Update(FinancialEntryEntity financialEntryEntity, DayBalanceEntity dayBalanceEntity);
        Task<DayBalanceEntity> FillDayBalance(FinancialEntryEntity entity, DayBalanceEntity dayBalanceEntity = null);
        decimal CalculateInterest(DayBalanceEntity entity);
        decimal CalculateBalance(DayBalanceEntity entity);
    }
}
