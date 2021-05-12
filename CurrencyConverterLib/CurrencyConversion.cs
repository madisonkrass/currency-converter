using System;

namespace CurrencyConverterLib
{
    public class CurrencyConversion
    {
        public CurrencyConversion(string currencyCode, string currencyName, decimal rateFromUSDToCurrency)
        {
            CurrencyCode = currencyCode;
            CurrencyName = currencyName;
            RateFromUSDToCurrency = rateFromUSDToCurrency;
        }
        public string CurrencyCode { get; private set; }
        public string CurrencyName { get; private set; }
        public decimal RateFromUSDToCurrency { get; private set; }
    }
}