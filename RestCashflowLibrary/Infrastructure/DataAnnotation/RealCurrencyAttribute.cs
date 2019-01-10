using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RestCashflowLibrary.Infrastructure.DataAnnotation
{
    public class RealCurrencyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string strValue = value as string;
            if (strValue == null)
            {
                return true;
            }
            var regex = new Regex(@"^R\$ (\d{1,3}(\.\d{3})*|\d+)(\,\d{2})?$");
            return regex.Match(strValue).Success;
        }
    }
}
