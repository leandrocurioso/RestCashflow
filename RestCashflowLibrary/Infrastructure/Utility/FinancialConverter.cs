using System;
using System.Globalization;

namespace RestCashflowLibrary.Infrastructure.Utility
{
    public static class FinancialConverter
    {
        public static string ToRealFormat(decimal value)
        {
            var result =  value.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            return result.Insert(2, " ");
        }

        public static decimal FromRealToDecimal(string value)
        {
            if (value == null)
            {
                value = "0,00";
            }
            var clearedValue = value.Replace("R$ ", string.Empty);
            clearedValue = clearedValue.Replace(".", string.Empty);
            return Convert.ToDecimal(clearedValue);
        }

        public static string ToStringPercentage(decimal value)
        {
            value = Math.Round(value, 2);
            return value.ToString("0.00") + "%";
        }

    }
}
