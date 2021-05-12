using System;
using System.Collections.Generic;

namespace CurrencyConverterLib
{
    public interface ICurrencyConverterRepository
    {
        IEnumerable<CurrencyConversion> GetConversions();
    }
}