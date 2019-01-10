using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestCashflowLibrary.Domain.Model.Entity
{
    [Table("day_balance")]
    public class DayBalanceEntity
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Required]
        [Column("total_entry")]
        [DataType(DataType.Currency)]
        public decimal TotalEntry { get; set; }

        [Required]
        [Column("total_out")]
        [DataType(DataType.Currency)]
        public decimal TotalOut { get; set; }

        [Required]
        [Column("interest")]
        [DataType(DataType.Currency)]
        public decimal Interest { get; set; }

        public virtual decimal Balance { get; set; }
    }
}
