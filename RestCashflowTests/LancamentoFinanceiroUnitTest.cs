using RestCashflowLibrary.Domain.Model.DataTransferObject;
using Xunit;
using RestCashflowLibrary.Domain.Model.Enum;
using RestCashflowLibrary.Domain.Model.Entity;
using RestCashflowWebApi.Model;

namespace RestCashflowTests
{
    public class LancamentoFinanceiroUnitTest
    {
        static LancamentoFinanceiroUnitTest()
        {
            Setup.Initialize();
        }

        [Fact]
        public void ToFinancialEntryEntity()
        {
            var dto = new LancamentoFinanceiro
            {
                descricao = "Test description",
                banco_destino = BankEnum.NuBank,
                conta_destino = "0001",
                tipo_de_conta = AccountTypeEnum.Checking,
                cpf_cnpj_destino = "47.972.568/0001-38",
                tipo_da_lancamento = FinancialEntryTypeEnum.Payment,
                valor_do_lancamento = "R$ 1.000,05",
                data_de_lancamento = "05-01-2019"
            };

            var entity = (FinancialEntryEntity)dto;

            Assert.Equal("Test description", entity.Description);
            Assert.Equal(BankEnum.NuBank, entity.DestinationBank);
            Assert.Equal("0001", entity.DestinationAccount);
            Assert.Equal(AccountTypeEnum.Checking, entity.AccountType);
            Assert.Equal("47.972.568/0001-38", entity.DestinationCpfCnpj);
            Assert.Equal(FinancialEntryTypeEnum.Payment, entity.EntryType);
            Assert.Equal(1000.05m, entity.Value);
            Assert.Equal("05-01-2019", entity.EntryDate.ToString("dd-MM-yyyy"));
        }
    }
}
