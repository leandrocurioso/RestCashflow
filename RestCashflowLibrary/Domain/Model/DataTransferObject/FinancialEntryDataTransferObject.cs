using System;
using System.ComponentModel.DataAnnotations;
using RestCashflowLibrary.Domain.Model.Enum;
using RestCashflowLibrary.Infrastructure.DataAnnotation;

namespace RestCashflowLibrary.Domain.Model.DataTransferObject
{
    [Serializable]
    public class FinancialEntryDataTransferObject
    {
        [Required]
        [EnumDataType(typeof(FinancialEntryTypeEnum))]
        public FinancialEntryTypeEnum EntryType { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [RegularExpression("^[0-9]+$")]
        public string DestinationAccount { get; set; }

        [Required]
        [EnumDataType(typeof(BankEnum))]
        public BankEnum DestinationBank { get; set; }

        [Required]
        [EnumDataType(typeof(AccountTypeEnum))]
        public AccountTypeEnum AccountType { get; set; }

        [Required]
        [CpfOrCnpj(WithMask = true)]
        public string DestinationCpfCnpj { get; set; }

        [Required]
        public decimal Value { get; set; }

        public decimal Charge { get; set; }

        [Required]
        public DateTime EntryDate { get; set; }

    }

}
