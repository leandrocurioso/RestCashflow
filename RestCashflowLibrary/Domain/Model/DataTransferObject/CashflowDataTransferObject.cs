using System;
using System.Collections.Generic;

namespace RestCashflowLibrary.Domain.Model.DataTransferObject
{
    public class EntryCashflow
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }

    public class OutCashflow
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }

    public class ChargeCashflow
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }

    [Serializable]
    public class CashflowDataTransferObject
    {
        public DateTime Date { get; set; }
        public IEnumerable<EntryCashflow> Entries { get; set; }
        public IEnumerable<OutCashflow> Outs { get; set; }
        public IEnumerable<ChargeCashflow> Charges { get; set; }
        public decimal Total { get; set; }
        public decimal DayPosition { get; set; }
    }
}
