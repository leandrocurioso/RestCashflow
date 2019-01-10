using System.Globalization;

namespace RestCashflowTests
{
    public static class Setup
    {
        public static void Initialize()
        {
            SetCulture();
        }

        public static void SetCulture()
        {
            var cultureInfo = new CultureInfo("pt-BR");
            cultureInfo.NumberFormat.CurrencySymbol = "R$";
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

    }
}
