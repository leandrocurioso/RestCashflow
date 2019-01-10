using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestCashflowLibrary.Domain.Model.DataTransferObject;
using RestCashflowLibrary.Domain.Model.Entity;
using RestCashflowLibrary.Domain.Model.Enum;
using RestCashflowLibrary.Domain.Service;
using RestCashflowLibrary.Infrastructure.CustomException;
using RestCashflowLibrary.Infrastructure.Queue;
using RestCashflowLibrary.Infrastructure.Repository;
using RestCashflowLibrary.Infrastructure.Utility;
using System.Linq;

namespace RestCashflowLibrary.Domain.Business
{
    public class FinancialEntryBusiness : IFinancialEntryBusiness
    {
        readonly IPaymentQueue _paymentQueue;
        readonly IReceiptQueue _receiptQueue;
        readonly IFinancialEntryRepository _financialEntryRepository;
        readonly IFinancialEntryValidateService _financialEntryValidateService;
        readonly IDayBalanceBusiness _dayBalanceBusiness;

        public IConfiguration Configuration { get; set; }

        public FinancialEntryBusiness(
            IPaymentQueue paymentQueue,
            IReceiptQueue receiptQueue,
            IFinancialEntryRepository financialEntryRepository,
            IFinancialEntryValidateService financialEntryValidateService,
            IDayBalanceBusiness dayBalanceBusiness,
            IConfiguration configuration
        )
        {
            _paymentQueue = paymentQueue;
            _receiptQueue = receiptQueue;
            _financialEntryRepository = financialEntryRepository;
            _financialEntryValidateService = financialEntryValidateService;
            _dayBalanceBusiness = dayBalanceBusiness;
            Configuration = configuration;
        }

        public async Task AddToQueue(FinancialEntryEntity financialEntryEntity)
        {
            try
            {
                var dayAccountLimit = Convert.ToDecimal(Configuration.GetSection("Business:DayAccountLimit").Value);
                if (_financialEntryValidateService.IsEntryInThePast(financialEntryEntity.EntryDate, DateTime.Now))
                {
                    throw new ApiException("Você não pode fazer lançamentos no passado!", HttpStatusCode.BadRequest);
                }

                if (financialEntryEntity.EntryType == FinancialEntryTypeEnum.Payment)
                {
                    var dayBalanceEntity = await _dayBalanceBusiness.FillDayBalance(financialEntryEntity);
                    if (_financialEntryValidateService.IsDayAccountLimitReached(dayBalanceEntity))
                    {
                        throw new ApiException(string.Format("O valor ultrapassa o limite de pagamento diário no valor de {0}.", FinancialConverter.ToRealFormat(dayAccountLimit)), HttpStatusCode.UnprocessableEntity);
                    }

                    await _paymentQueue.Publish(financialEntryEntity);
                }

                if (financialEntryEntity.EntryType == FinancialEntryTypeEnum.Receipt)
                {
                    await _receiptQueue.Publish(financialEntryEntity);
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<long> Create(FinancialEntryEntity entity)
        {
            try
            {
                return await _financialEntryRepository.Create(entity);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<CashflowDataTransferObject>> GetCashflow(DateTime startDate, int daysAhead)
        {
            try
            {
                var resultList = new List<CashflowDataTransferObject>();

                var entries = await _financialEntryRepository.ListConciledBetweenDate(startDate.AddDays(-1), startDate.AddDays(daysAhead));
                if (entries == null || !entries.Any())
                {
                    return resultList;
                }

                var availableDates = new List<DateTime>();
                foreach (var entry in entries)
                {
                    if (!availableDates.Contains(entry.EntryDate))
                    {
                        availableDates.Add(entry.EntryDate);
                    }
                }

                foreach (var currentDate in availableDates)
                {
                    var filteredEntries = entries.Where(x => x.EntryDate.Date == currentDate.Date).ToList();
                    var payments = filteredEntries.Where(x => x.EntryType == FinancialEntryTypeEnum.Payment).ToList();
                    var receipts = filteredEntries.Where(x => x.EntryType == FinancialEntryTypeEnum.Receipt).ToList();

                    var dto = new CashflowDataTransferObject()
                    {
                        Date = currentDate,
                        Entries = receipts.ToList().ConvertAll(r => new EntryCashflow()
                        {
                            Date = r.ConciledAt,
                            Value = r.Value
                        }),
                        Outs = payments.ToList().ConvertAll(o => new OutCashflow()
                        {
                            Date = o.ConciledAt,
                            Value = o.Value
                        }),
                        Charges = filteredEntries.ToList().ConvertAll(c => new ChargeCashflow()
                        {
                            Date = c.ConciledAt,
                            Value = c.Charge
                        }),
                        Total = (filteredEntries.Sum(x => x.Value) + filteredEntries.Sum(x => x.Charge))
                    };
                    // Position day
                    var yesterday = dto.Date.AddDays(-1);
                    var foundYesterday = resultList.Find(x => x.Date.Date == yesterday.Date);
                    dto.DayPosition = CalculateDayPositionPercentage(foundYesterday?.Total, dto.Total);
                    resultList.Add(dto);
                }
                return resultList.Where(x => x.Date.Date >= DateTime.Now.Date).OrderBy(x => x.Date).ToList();
            }
            catch
            {
                throw;
            }
        }

        public decimal CalculateDayPositionPercentage(decimal? yesterdayTootal, decimal currentDayTotal)
        {
            if (yesterdayTootal == null)
            {
                return 100;
            }
            var diff = currentDayTotal - yesterdayTootal;
            return (decimal)((decimal)(diff * 100) / yesterdayTootal);
        }
    }
}
