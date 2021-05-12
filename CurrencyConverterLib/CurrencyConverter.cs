using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyConverterLib
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private ICurrencyConverterRepository _repository;
        public CurrencyConverter(ICurrencyConverterRepository repository)
        {
            _repository = repository;
        }
        public decimal GetConvertedAmount(string fromCountryCode, string toCountryCode, decimal fromAmount)
        {
            //Could get conversions one time at instantiation, but getting them here assuming they may change
            var conversions = _repository.GetConversions();
            var fromConversion = conversions.SingleOrDefault(x => x.CurrencyCode.Equals(fromCountryCode, StringComparison.CurrentCultureIgnoreCase));
            var toConversion = conversions.SingleOrDefault(x => x.CurrencyCode.Equals(toCountryCode, StringComparison.CurrentCultureIgnoreCase));

            if (fromConversion == null)
            {
                throw new InvalidCurrencyException($"{fromCountryCode} is not a recognized country code.");
            }
            else if (toConversion == null)
            {
                throw new InvalidCurrencyException($"{toCountryCode} is not a recognized country code.");
            }
            if (fromConversion.RateFromUSDToCurrency <= 0)
            {
                throw new InvalidCurrencyException($"{fromConversion.CurrencyCode} has an invalid rate of {fromConversion.RateFromUSDToCurrency}");
            }
            else if (toConversion.RateFromUSDToCurrency <= 0)
            {
                throw new InvalidCurrencyException($"{toConversion.CurrencyCode} has an invalid rate of {toConversion.RateFromUSDToCurrency}");
            }
            return fromAmount * (toConversion.RateFromUSDToCurrency / fromConversion.RateFromUSDToCurrency);
        }

        public IEnumerable<CurrencyConversion> GetCurrencies()
        {
            return _repository.GetConversions();
        }
    }
}
