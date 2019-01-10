using RestCashflowLibrary.Infrastructure.Utility;
using Xunit;

namespace RestCashflowTests
{
    public class GeneralValidatorUnitTest
    {
        static GeneralValidatorUnitTest()
        {
            Setup.Initialize();
        }

        [Theory]
        [InlineData("031.484.711-13")]
        [InlineData("03148471113")]
        [InlineData("315.659.250-17")]
        [InlineData("31565925017")]
        public void IsValidCpf(string cpf)
        {
            Assert.True(GeneralValidator.IsCpf(cpf));
        }

        [Theory]
        [InlineData("031.484.711-133")]
        [InlineData("031484711133")]
        [InlineData("315.659.250-173")]
        [InlineData("315659250173")]
        public void IsInvalidCpf(string cpf)
        {
            Assert.False(GeneralValidator.IsCpf(cpf));
        }

        [Theory]
        [InlineData("48.206.861/0001-57")]
        [InlineData("48206861000157")]
        [InlineData("67.734.409/0001-02")]
        [InlineData("67734409000102")]
        public void IsValidCnpj(string cnpj)
        {
            Assert.True(GeneralValidator.IsCnpj(cnpj));
        }

        [Theory]
        [InlineData("48.206.861/0001-570")]
        [InlineData("482068610001570")]
        [InlineData("67.734.409/0001-020")]
        [InlineData("677344090001020")]
        public void IsInvalidCnpj(string cnpj)
        {
            Assert.False(GeneralValidator.IsCnpj(cnpj));
        }
    }
}
