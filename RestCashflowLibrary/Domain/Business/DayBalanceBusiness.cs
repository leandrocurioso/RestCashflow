using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestCashflowLibrary.Domain.Model.Entity;
using RestCashflowLibrary.Domain.Model.Enum;
using RestCashflowLibrary.Domain.Service;
using RestCashflowLibrary.Infrastructure.Repository;

namespace RestCashflowLibrary.Domain.Business
{
    public class DayBalanceBusiness : IDayBalanceBusiness
    {
        readonly IFinancialEntryValidateService _financialEntryValidateService;
        readonly IDayBalanceRepository _dayBalanceRepository;
        public IConfiguration Configuration { get; set; }

        public DayBalanceBusiness(
            IDayBalanceRepository dayBalanceRepository,
            IFinancialEntryValidateService financialEntryValidateService,
            IConfiguration configuration
        )
        {
            _dayBalanceRepository = dayBalanceRepository;
            _financialEntryValidateService = financialEntryValidateService;
            Configuration = configuration;
        }

        public async Task<long> Insert(FinancialEntryEntity entity)
        {
            try
            {
                var isPayment = entity.EntryType == FinancialEntryTypeEnum.Payment;
                var dayBalanceEntity = await FillDayBalance(entity);
                return await _dayBalanceRepository.Insert(dayBalanceEntity);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Update(FinancialEntryEntity financialEntryEntity, DayBalanceEntity dayBalanceEntity)
        {
            try
            {
                var dayInterest = Convert.ToDecimal(Configuration.GetSection("Business:DayInterest").Value);
                dayBalanceEntity = await FillDayBalance(financialEntryEntity, dayBalanceEntity);
                return await _dayBalanceRepository.Update(dayBalanceEntity);
            }
            catch
            {
                throw;
            }
        }

        public async Task<DayBalanceEntity> FillDayBalance(FinancialEntryEntity entity, DayBalanceEntity dayBalanceEntity = null)
        {
            try
            {
                var isPayment = entity.EntryType == FinancialEntryTypeEnum.Payment;
                if (dayBalanceEntity == null)
                {
                    dayBalanceEntity = await _dayBalanceRepository.GetByDate(entity.EntryDate);
                }

                if (dayBalanceEntity == null)
                {
                    dayBalanceEntity = new DayBalanceEntity();
                }

                dayBalanceEntity.Date = entity.EntryDate;

                if (isPayment)
                {
                    dayBalanceEntity.TotalOut += entity.Value;
                }
                else
                {
                    dayBalanceEntity.TotalEntry += entity.Value;
                }

                dayBalanceEntity.Balance = CalculateBalance(dayBalanceEntity);
                dayBalanceEntity.Interest = CalculateInterest(dayBalanceEntity);
                return dayBalanceEntity;
            }
            catch
            {
                throw;
            }
        }

        public decimal CalculateInterest(DayBalanceEntity entity)
        {
            decimal interest = 0;
            var dayInterest = Convert.ToDecimal(Configuration.GetSection("Business:DayInterest").Value);
            if (entity.Balance < 0)
            {
                interest = ((entity.Balance * dayInterest) / 100) * -1;
            }
            return interest;
        }

        public decimal CalculateBalance(DayBalanceEntity entity)
        {
            return entity.TotalEntry - entity.TotalOut;
        }
    }
}
