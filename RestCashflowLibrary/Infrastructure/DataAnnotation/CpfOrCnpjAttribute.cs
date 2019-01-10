using System.ComponentModel.DataAnnotations;
using RestCashflowLibrary.Infrastructure.Utility;

namespace RestCashflowLibrary.Infrastructure.DataAnnotation
{
    public class CpfOrCnpjAttribute : ValidationAttribute
    {
        public bool WithMask { get; set; }

        public CpfOrCnpjAttribute()
        {
            WithMask = false;
        }

        public override bool IsValid(object value)
        {
            var strValue = value as string;
            var cpfLengthWithMask = 14;
            var cpfLengthWithoutMask = 11;
            var cnpjLengthWithMask = 18;
            var cnpjLengthWithoutMask = 14;

            if (strValue == null)
            {
                return true;
            }

            if (GeneralValidator.IsCnpj(strValue))
            {
                if (WithMask && strValue.Length != cnpjLengthWithMask)
                {
                    return false;
                }

                if (!WithMask && strValue.Length != cnpjLengthWithoutMask)
                {
                    return false;
                }

                return true;
            }

            if (GeneralValidator.IsCpf(strValue))
            {
                if (WithMask && strValue.Length != cpfLengthWithMask)
                {
                    return false;
                }

                if (!WithMask && strValue.Length != cpfLengthWithoutMask)
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}