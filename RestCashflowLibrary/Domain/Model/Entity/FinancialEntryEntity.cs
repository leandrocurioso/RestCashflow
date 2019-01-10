using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestCashflowLibrary.Domain.Model.Enum;
using RestCashflowLibrary.Infrastructure.DataAnnotation;

namespace RestCashflowLibrary.Domain.Model.Entity
{
    [Table("financial_entry")]
    public class FinancialEntryEntity
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("entry_type")]
        public FinancialEntryTypeEnum EntryType { get; set; }

        [Required]
        [Column("description")]
        public string Description { get; set; }

        [Required]
        [Column("destination_account")]
        public string DestinationAccount { get; set; }

        [Required]
        [Column("destination_bank")]
        public BankEnum DestinationBank { get; set; }

        [Required]
        [CpfOrCnpj(WithMask = false)]
        [Column("destination_cpf_cnpj")]
        public string DestinationCpfCnpj { get; set; }

        [Required]
        [Column("account_type")]
        public AccountTypeEnum AccountType { get; set; }

        [Required]
        [Column("value")]
        [DataType(DataType.Currency)]
        public decimal Value { get; set; }

        [Required]
        [Column("charge")]
        [DataType(DataType.Currency)]
        public decimal Charge { get; set; }

        [Required]
        [Column("entry_date")]
        public DateTime EntryDate { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt = DateTime.Now;

        [Required]
        [Column("conciled")]
        public bool Conciled = false;

        [Column("conciled_at")]
        public DateTime ConciledAt { get; set; }

    }
}