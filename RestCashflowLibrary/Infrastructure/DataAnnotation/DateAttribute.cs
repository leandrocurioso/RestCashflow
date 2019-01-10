using System;
using System.ComponentModel.DataAnnotations;

namespace RestCashflowLibrary.Infrastructure.DataAnnotation
{
    public class DateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string strDate = value as string;
            char separator = '-';
            char firstSeparator = strDate[2];
            char secondSeparator = strDate[5];
            if (strDate.Length != 10 || firstSeparator != separator || secondSeparator != separator)
            {
                return false;
            }
            if (!DateTime.TryParse(strDate, out DateTime dt))
            {
                return false;
            }
            return true;
        }
    }
}
