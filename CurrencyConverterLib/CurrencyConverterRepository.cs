using System;
using System.Collections.Generic;

namespace CurrencyConverterLib
{
    public class CurrencyConverterRepository : ICurrencyConverterRepository
    {
        public IEnumerable<CurrencyConversion> GetConversions()
        {
            return new[] {
                new CurrencyConversion("USD", "United States Dollars", 1.0M),
                new CurrencyConversion("CAD", "Canada Dollars", 1.2071M),
                new CurrencyConversion("MXN", "Mexico Pesos", 15.22M),
                new CurrencyConversion("CRC", "Costa Rica Colons", 538.52M),
                new CurrencyConversion("DZD", "Algeria Dinars", 97.56M),
                new CurrencyConversion("CNY", "China Renminbis", 6.08M),
                new CurrencyConversion("DKK", "Denmark Krones", 6.6181M),
                new CurrencyConversion("PLN", "Poland Zlotys", 3.6284M)
            };
        }
    }
}