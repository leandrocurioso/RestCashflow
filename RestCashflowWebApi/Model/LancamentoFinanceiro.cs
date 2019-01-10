using System;
using System.ComponentModel.DataAnnotations;
using RestCashflowLibrary.Domain.Model.Entity;
using RestCashflowLibrary.Domain.Model.Enum;
using RestCashflowLibrary.Infrastructure.DataAnnotation;
using RestCashflowLibrary.Infrastructure.Utility;

namespace RestCashflowWebApi.Model
{
    [Serializable]
    public class LancamentoFinanceiro
    {
        /// <summary>
        /// Tipo do lançamento
        /// </summary>
        /// <example>Pagamento = 0, Recebimento = 1</example>
        [Required(ErrorMessage = "O campo tipo de lançamento é requerido.")]
        [EnumDataType(typeof(FinancialEntryTypeEnum), ErrorMessage = "O tipo de conta não é válido.")]
        public FinancialEntryTypeEnum tipo_da_lancamento { get; set; }

        /// <summary>
        /// Descrição do lançamento
        /// </summary>
        /// <example>Pagamento da conta de luz</example>
        [Required(ErrorMessage = "O campo descrição é requerido.")]
        [MaxLength(1000)]
        public string descricao { get; set; }

        /// <summary>
        /// Conta bancária de destino
        /// </summary>
        /// <example>00001</example>
        [Required(ErrorMessage = "O campo conta destino é requerido.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "O campo conta destino deve conter somente números.")]
        public string conta_destino { get; set; }

        /// <summary>
        /// Banco de destino
        /// </summary>
        /// <example>Itau = 0, Bradesco = 1, Santander = 2, NuBank = 4, Caixa = 5</example>
        [Required(ErrorMessage = "O campo banco destino é requerido.")]
        [EnumDataType(typeof(BankEnum), ErrorMessage = "O banco destino não é válido.")]
        public BankEnum banco_destino { get; set; }

        /// <summary>
        /// Tipo de conta
        /// </summary>
        /// <example>Conta Corrente = 0, Conta Poupança = 1</example>
        [Required(ErrorMessage = "O campo tipo de conta é requerido.")]
        [EnumDataType(typeof(AccountTypeEnum), ErrorMessage = "O tipo de conta não é válido.")]
        public AccountTypeEnum tipo_de_conta { get; set; }

        /// <summary>
        /// CPF ou CNPJ de Destino
        /// </summary>
        /// <example>836.885.190-43 ou 94.990.012/0001-54</example>
        [Required(ErrorMessage = "O campo CPF/CNPJ destino é requerido.")]
        [CpfOrCnpj(WithMask = true, ErrorMessage = "O CPF/CNPJ destino não é válido.")]
        public string cpf_cnpj_destino { get; set; }

        /// <summary>
        /// Valor do lançamento
        /// </summary>
        /// <example>R$ 1.000,00</example>
        [Required(ErrorMessage = "O campo valor do lançamento é requerido.")]
        [RealCurrency(ErrorMessage = "O formato deve ser R$ x.xxx,xx")]
        public string valor_do_lancamento { get; set; }

        /// <summary>
        /// Valor total dos encargos
        /// </summary>
        /// <example>R$ 500,00</example>
        [RealCurrency(ErrorMessage = "O formato deve ser R$ x.xxx,xx")]
        public string encargos { get; set; }

        /// <summary>
        /// Data do lançamento
        /// </summary>
        /// <example>01-01-2019</example>
        [Required(ErrorMessage = "O campo data de lançamento é requerido.")]
        [Date(ErrorMessage = "O data de lançamento deve ser o fomato dd-mm-aaaa")]
        public string data_de_lancamento { get; set; }

        public static explicit operator FinancialEntryEntity(LancamentoFinanceiro model)
        {
            return new FinancialEntryEntity
            {
                EntryDate = Convert.ToDateTime(model.data_de_lancamento),
                EntryType = model.tipo_da_lancamento,
                Description = model.descricao,
                DestinationAccount = model.conta_destino,
                DestinationBank = model.banco_destino,
                AccountType = model.tipo_de_conta,
                DestinationCpfCnpj = model.cpf_cnpj_destino,
                Value = FinancialConverter.FromRealToDecimal(model.valor_do_lancamento),
                Charge = FinancialConverter.FromRealToDecimal(model.encargos)
            };
        }
    }
}
