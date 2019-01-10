using RestCashflowLibrary.Infrastructure.Utility;
using Xunit;

namespace RestCashflowTests
{
    public class FinancialConverterUnitTest
    {
        static FinancialConverterUnitTest()
        {
            Setup.Initialize();
        }

        [Fact]
        public void ToRealFormat()
        {
            var result1 = FinancialConverter.ToRealFormat(1000);
            var result2 = FinancialConverter.ToRealFormat(50.25m);
            var result3 = FinancialConverter.ToRealFormat(200.250m);
            var result4 = FinancialConverter.ToRealFormat(4500.50m);
            Assert.Equal("R$ 1.000,00", result1);
            Assert.Equal("R$ 50,25", result2);
            Assert.Equal("R$ 200,25", result3);
            Assert.Equal("R$ 4.500,50", result4);
        }

        [Fact]
        public void FromRealToDouble()
        {
            var result1 = FinancialConverter.FromRealToDecimal("R$ 1.000,00");
            var result2 = FinancialConverter.FromRealToDecimal("R$ 1.000.000,00");
            var result3 = FinancialConverter.FromRealToDecimal("25,25");
            var result4 = FinancialConverter.FromRealToDecimal("500,000");
            Assert.Equal(1000, result1);
            Assert.Equal(1000000, result2);
            Assert.Equal(25.25m, result3);
            Assert.Equal(500, result4);
        }
    }
}
