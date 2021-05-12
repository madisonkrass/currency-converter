using System;

namespace CurrencyConverterLib
{
    public class InvalidCurrencyException : Exception
    {
        public InvalidCurrencyException(string message)
            : base(message)
        {
            
        }
    }
}