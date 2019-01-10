using Microsoft.Extensions.Configuration;
using Moq;
using RestCashflowLibrary.Domain.Business;
using RestCashflowLibrary.Domain.Model.Entity;
using RestCashflowLibrary.Domain.Model.Enum;
using RestCashflowLibrary.Domain.Service;
using RestCashflowLibrary.Infrastructure.Repository;
using Xunit;

namespace RestCashflowTests
{
    public class DayBalanceBusinessUnitTest
    {
        Mock<IFinancialEntryValidateService> _financialEntryValidateService = new Mock<IFinancialEntryValidateService>();
        Mock<IDayBalanceRepository> _dayBalanceRepository = new Mock<IDayBalanceRepository>();
        Mock<IConfiguration> _configuration = new Mock<IConfiguration>();

        static DayBalanceBusinessUnitTest()
        {
            Setup.Initialize();
        }

        [Theory]
        [InlineData("0,83", -1, 0.0083)]
        [InlineData("5,00", -10, 0.5)]
        [InlineData("10,00", -100, 10)]
        [InlineData("0,01", -1000, 0.1)]
        [InlineData("50,00", 10000, 0)]
        public void CalculateInterest(string interest, decimal balance, decimal percentage)
        {
            _configuration.Setup(x => x.GetSection("Business:DayInterest").Value).Returns(interest);
            var dayBalanceBusiness = new DayBalanceBusiness(_dayBalanceRepository.Object, _financialEntryValidateService.Object, _configuration.Object);
            var entiry = new DayBalanceEntity()
            {
                Balance = balance
            };

            var result = dayBalanceBusiness.CalculateInterest(entiry);
            Assert.Equal(percentage, result);
        }

        [Theory]
        [InlineData(1000, 500, -500)]
        [InlineData(2000, 1000, -1000)]
        [InlineData(0.5, 0.25, -0.25)]
        [InlineData(500, 1750, 1250)]
        [InlineData(100, 100, 0)]
        [InlineData(0.1, 0.2, 0.1)] 
        public void CalculateBalance(decimal totalOut, decimal totalEntry, decimal expected)
        {
            var dayBalanceBusiness = new DayBalanceBusiness(_dayBalanceRepository.Object, _financialEntryValidateService.Object, _configuration.Object);
            var entiry = new DayBalanceEntity()
            {
                TotalOut = totalOut,
                TotalEntry = totalEntry
            };
            var result = dayBalanceBusiness.CalculateBalance(entiry);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(500, 500, 1000, 0, 0)]
        [InlineData(1000, 250, 1500, 250, 0)]
        [InlineData(0, 1000, 500, -500, 4.15)]
        public void FillDayBalance(decimal currentValue, decimal totalOut, decimal totalEntry, decimal expectedBalance, decimal expectedInterest)
        {
            _configuration.Setup(x => x.GetSection("Business:DayInterest").Value).Returns("0,83");
            var dayBalanceBusiness = new DayBalanceBusiness(_dayBalanceRepository.Object, _financialEntryValidateService.Object, _configuration.Object);
            var financialEntryEntity = new FinancialEntryEntity() {
                EntryType = FinancialEntryTypeEnum.Payment,
                Value = currentValue
            };
            var dayBalanceEntity = new DayBalanceEntity()
            {
                TotalOut = totalOut,
                TotalEntry = totalEntry
            };
            var result = dayBalanceBusiness.FillDayBalance(financialEntryEntity, dayBalanceEntity).GetAwaiter().GetResult();
            Assert.Equal(expectedBalance, result.Balance);
            Assert.Equal(expectedInterest, result.Interest);
            Assert.Equal((currentValue + totalOut), result.TotalOut);
            Assert.Equal(totalEntry, result.TotalEntry);
        }
    }
}
