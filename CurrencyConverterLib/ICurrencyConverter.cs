using System;
using System.Collections.Generic;

namespace CurrencyConverterLib
{
    public interface ICurrencyConverter
    {
        decimal GetConvertedAmount(string fromCountryCode, string toCountryCode, decimal fromAmount);
        IEnumerable<CurrencyConversion> GetCurrencies();
    }
}