using System;
using Microsoft.Extensions.Configuration;
using Moq;
using RestCashflowLibrary.Domain.Business;
using RestCashflowLibrary.Domain.Model.Entity;
using RestCashflowLibrary.Domain.Service;
using RestCashflowLibrary.Infrastructure.Repository;
using Xunit;

namespace RestCashflowTests
{
    public class FinancialEntryValidateServiceUnitTest
    {
        Mock<IDayBalanceRepository> _dayBalanceRepository = new Mock<IDayBalanceRepository>();
        Mock<IDayBalanceBusiness> _dayBalanceBusiness = new Mock<IDayBalanceBusiness>();
        Mock<IConfiguration> _configuration = new Mock<IConfiguration>();

        static FinancialEntryValidateServiceUnitTest()
        {
            Setup.Initialize();
        }

        [Theory]
        [InlineData("01-01-2019", "02-01-2019")]
        [InlineData("01-01-2020", "02-01-2021")]
        [InlineData("01-05-2019", "02-06-2019")]
        [InlineData("01-10-2019", "02-11-2019")]
        public void IsEntryInThePast(string currentDt, string now)
        {
            var financialEntryValidateService = new FinancialEntryValidateService(_configuration.Object);
            var result = financialEntryValidateService.IsEntryInThePast(Convert.ToDateTime(currentDt), Convert.ToDateTime(now));
            Assert.True(result);
        }

        [Theory]
        [InlineData("10000", 21000)]
        [InlineData("15000", 16000)]
        [InlineData("5000", 5001)]
        [InlineData("2500", 2501)]
        public void IsDayAccountLimitReached(string dayAccountLimit, decimal value)
        {
            _configuration.Setup(x => x.GetSection("Business:DayAccountLimit").Value).Returns(dayAccountLimit);
            var dayBalanceEntity = new DayBalanceEntity() 
            {
                Balance  = value
            };
            var financialEntryValidateService = new FinancialEntryValidateService(_configuration.Object);
            var result = financialEntryValidateService.IsDayAccountLimitReached(dayBalanceEntity);
            Assert.True(result);
        }

    }
}
