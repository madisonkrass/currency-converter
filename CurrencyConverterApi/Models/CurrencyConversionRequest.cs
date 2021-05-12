using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CurrencyConverterApi.Models
{
    public class CurrencyConversionRequest
    {
        [BindRequired]
        public string From { get; set; }

        [BindRequired]
        public string To { get; set; }

        [BindRequired]
        public decimal Amount { get; set; }
    }
}