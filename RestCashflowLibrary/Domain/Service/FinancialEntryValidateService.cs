using System;
using Microsoft.Extensions.Configuration;
using RestCashflowLibrary.Domain.Model.Entity;

namespace RestCashflowLibrary.Domain.Service
{
    public class FinancialEntryValidateService : IFinancialEntryValidateService
    {
        public IConfiguration Configuration { get; set; }

        public FinancialEntryValidateService(
            IConfiguration configuration
        )
        {
            Configuration = configuration;
        }

        public bool IsEntryInThePast(DateTime currentDt, DateTime now)
        {
            return currentDt.Date < now.Date;
        }

        public bool IsDayAccountLimitReached(DayBalanceEntity dayBalanceEntity)
        {
            try
            {
                var dayAccountLimit = Convert.ToDecimal(Configuration.GetSection("Business:DayAccountLimit").Value);
                return dayBalanceEntity.Balance > dayAccountLimit;
            }
            catch
            {
                throw;
            }
        }
    }
}
