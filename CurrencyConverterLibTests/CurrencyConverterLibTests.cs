using NUnit.Framework;
using CurrencyConverterLib;
using System.Collections.Generic;
using System.Linq;
using System;
using Moq;

namespace CurrencyConverterLibTests
{
    public class Tests
    {
        private ICurrencyConverterRepository _repository;
        private ICurrencyConverter _converter;

        [SetUp]
        public void Setup()
        {
            var mock = new Mock<ICurrencyConverterRepository>();
            mock.Setup(x => x.GetConversions()).Returns(
                new[]
                {
                    new CurrencyConversion("USD", "United States Dollars", 1.0M),
                    new CurrencyConversion("CAD", "Canada Dollars", 1.2M),
                    new CurrencyConversion("ABC", "Test Currency", 0.5M),
                    new CurrencyConversion("ZER", "0 Rate", 0M),
                    new CurrencyConversion("MAX", "Max Value", decimal.MaxValue)
                }
            );
            _repository = mock.Object;
            _converter = new CurrencyConverter(_repository);
        }

        [Test]
        public void TestAllConversionCombinations()
        {
            var amount = 1.0M;
            var conversions = _repository.GetConversions().ToArray();
            var combinations = new List<(string, string)>();
            for (int i = 0; i < conversions.Count(); i++)
            {
                for (int j = i; j < conversions.Count(); j++)
                {
                    combinations.Add((conversions[i].CurrencyCode, conversions[j].CurrencyCode));
                }
            }

            foreach (var combination in combinations)
            {
                var fromConversion = _repository.GetConversions().SingleOrDefault(x => x.CurrencyCode == combination.Item1);
                var toConversion = _repository.GetConversions().SingleOrDefault(x => x.CurrencyCode == combination.Item2);
                
                //Filter out the currencies that I expect to fail.  A better way to handle this would be to setup different mock repositories
                //for each test case.
                if (fromConversion.RateFromUSDToCurrency > 0 && toConversion.RateFromUSDToCurrency > 0 && fromConversion.CurrencyCode != "MAX" && toConversion.CurrencyCode != "MAX")
                {
                    var actualResult = _converter.GetConvertedAmount(combination.Item1, combination.Item2, amount);
                    var expectedResult = amount * (toConversion.RateFromUSDToCurrency / fromConversion.RateFromUSDToCurrency);
                    Assert.AreEqual(expectedResult, actualResult, $"{amount}{combination.Item1}->{combination.Item2}");
                }
            }
        }

        [Test]
        public void TestInvalidFromConversionRate()
        {
            var amount = 10.0M;
            var conversion = _repository.GetConversions().Single(x => x.CurrencyCode == "ZER");
            Assert.Throws<InvalidCurrencyException>(() => _converter.GetConvertedAmount(conversion.CurrencyCode, "USD", amount));
        }

        [Test]
        public void TestInvalidToConversionRate()
        {
            var amount = 10.0M;
            var conversion = _repository.GetConversions().Single(x => x.CurrencyCode == "ZER");
            Assert.Throws<InvalidCurrencyException>(() => _converter.GetConvertedAmount("USD", conversion.CurrencyCode, amount));
        }

        [Test]
        public void TestInvalidFromCurrencyCode()
        {
            var from = "XYZ";
            var to = "USD";
            var amount = 1M;
            Assert.Throws<InvalidCurrencyException>(() => _converter.GetConvertedAmount(from, to, amount));
        }

        [Test]
        public void TestInvalidToCurrencyCode()
        {
            var from = "USD";
            var to = "XYZ";
            var amount = 1M;
            Assert.Throws<InvalidCurrencyException>(() => _converter.GetConvertedAmount(from, to, amount));
        }

        [Test]
        public void TestNullFromCurrencyCode()
        {
            string from = null;
            var to = "USD";
            var amount = 1M;
            Assert.Throws<InvalidCurrencyException>(() => _converter.GetConvertedAmount(from, to, amount));
        }

        [Test]
        public void TestNullToCurrencyCode()
        {
            var from = "USD";
            string to = null;
            var amount = 1M;
            Assert.Throws<InvalidCurrencyException>(() => _converter.GetConvertedAmount(from, to, amount));
        }

        [Test]
        public void TestNullCountryCodes()
        {
            string from = null;
            string to = null;
            var amount = 1.0M;
            Assert.Throws<InvalidCurrencyException>(() => _converter.GetConvertedAmount(from, to, amount));
        }

        [Test]
        public void TestSameCurrencyCode()
        {
            var code = "USD";
            var amount = 1M;
            Assert.AreEqual(amount, _converter.GetConvertedAmount(code, code, amount));
        }

        [Test]
        public void TestZeroAmount()
        {
            var code = "USD";
            var amount = 0M;
            Assert.AreEqual(amount, _converter.GetConvertedAmount(code, code, amount));
        }

        [Test]
        public void TestNegativeAmount()
        {
            var code = "USD";
            var amount = -1M;
            Assert.AreEqual(amount, _converter.GetConvertedAmount(code, code, amount));
        }

        [Test]
        public void TestOverflow()
        {
            var amount = 2M;
            Assert.Throws<OverflowException>(() => _converter.GetConvertedAmount("USD", "MAX", amount));
        }

        [Test]
        public void TestGetCurrencies()
        {
            var currencies = _converter.GetCurrencies();
            Assert.AreEqual(_repository.GetConversions().Count(), currencies.Count());
            Assert.AreEqual(currencies.All(x => _repository.GetConversions().SingleOrDefault(y => y.CurrencyCode == x.CurrencyCode) != null), true);
        }
    }
}