using System;
using System.Threading.Tasks;
using RestCashflowLibrary.Domain.Model.Entity;

namespace RestCashflowLibrary.Infrastructure.Repository
{
    public interface IDayBalanceRepository : IRepository
    {
        Task<DayBalanceEntity> GetByDate(DateTime dt);
        Task<long> Insert(DayBalanceEntity entity);
        Task<bool> Update(DayBalanceEntity entity);
    }
}
