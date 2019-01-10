using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using RestCashflowLibrary.Domain.Business;
using RestCashflowLibrary.Domain.Model.DataTransferObject;
using RestCashflowLibrary.Domain.Model.Entity;
using RestCashflowLibrary.Domain.Model.Enum;
using RestCashflowLibrary.Domain.Service;
using RestCashflowLibrary.Infrastructure.CustomException;
using RestCashflowLibrary.Infrastructure.Queue;
using RestCashflowLibrary.Infrastructure.Repository;
using Xunit;

namespace RestCashflowTests
{
    public class FinancialEntryBusinessUnitTest
    {
        Mock<IPaymentQueue> _paymentQueue = new Mock<IPaymentQueue>();
        Mock<IReceiptQueue> _receiptQueue = new Mock<IReceiptQueue>();
        Mock<IFinancialEntryRepository> _financialEntryRepository = new Mock<IFinancialEntryRepository>();
        Mock<IFinancialEntryValidateService> _financialEntryValidateService = new Mock<IFinancialEntryValidateService>();
        Mock<IDayBalanceBusiness> _dayBalanceBusiness = new Mock<IDayBalanceBusiness>();
        Mock<IConfiguration> _configuration = new Mock<IConfiguration>();

        static FinancialEntryBusinessUnitTest()
        {
            Setup.Initialize();
        }

        [Fact]
        public void AddToQueueNotDateInThePastException()
        {
            try
            {
                var now = DateTime.Now;
                var yesterday = now.AddDays(-1);
                _configuration.Setup(x => x.GetSection("Business:DayAccountLimit").Value).Returns("20000,00");
                _financialEntryValidateService.Setup(x => x.IsEntryInThePast(yesterday, now)).Returns(true);
                var financialEntryBusiness = new FinancialEntryBusiness(_paymentQueue.Object, _receiptQueue.Object, _financialEntryRepository.Object, _financialEntryValidateService.Object, _dayBalanceBusiness.Object, _configuration.Object);
                financialEntryBusiness.AddToQueue(new FinancialEntryEntity()).GetAwaiter().GetResult();
            }
            catch (ApiException ex)
            {
                Assert.Equal(HttpStatusCode.BadRequest, ex.HttpStatusCode);
            }
        }

        [Fact]
        public void AddToQueueDayAccountLimitException()
        {
            try
            {
                var financialEntryEntity = new FinancialEntryEntity()
                {
                    EntryType = FinancialEntryTypeEnum.Payment
                };
                var now = DateTime.Now;
                var tomorrow = now.AddDays(1);
                _configuration.Setup(x => x.GetSection("Business:DayAccountLimit").Value).Returns("20000,00");
                _financialEntryValidateService.Setup(x => x.IsEntryInThePast(tomorrow, now)).Returns(false);
                var financialEntryBusiness = new FinancialEntryBusiness(_paymentQueue.Object, _receiptQueue.Object, _financialEntryRepository.Object, _financialEntryValidateService.Object, _dayBalanceBusiness.Object, _configuration.Object);
                financialEntryBusiness.AddToQueue(financialEntryEntity).GetAwaiter().GetResult();
            }
            catch (ApiException ex)
            {
                Assert.Equal(HttpStatusCode.UnprocessableEntity, ex.HttpStatusCode);
            }
        }

        [Theory]
        [InlineData(100, 100, 0)]
        [InlineData(20, 40, 100)]
        [InlineData(40, 20, -50)]
        public void CalculateDayPositionPercentage(decimal yesterdayTootal, decimal currentDayTotal, decimal expectedResult)
        {
            var financialEntryBusiness = new FinancialEntryBusiness(_paymentQueue.Object, _receiptQueue.Object, _financialEntryRepository.Object, _financialEntryValidateService.Object, _dayBalanceBusiness.Object, _configuration.Object);
            var result = financialEntryBusiness.CalculateDayPositionPercentage(yesterdayTootal, currentDayTotal);
            Assert.Equal(expectedResult, result);
        }
    }
}
