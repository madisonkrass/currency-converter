using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CurrencyConverterLib;
using Microsoft.AspNetCore.Cors;

namespace CurrencyConverterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyConverterController : ControllerBase
    {

        private readonly ILogger<CurrencyConverterController> _logger;
        private readonly ICurrencyConverter _converter;

        public CurrencyConverterController(ILogger<CurrencyConverterController> logger, ICurrencyConverter currencyConverter)
        {
            _logger = logger;
            _converter = currencyConverter;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] Models.CurrencyConversionRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(_converter.GetConvertedAmount(request.From, request.To, request.Amount));
                }
                catch (InvalidCurrencyException e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest("Invalid request.");
        }

        [HttpGet]
        [Route("currencies")] 
        public IActionResult GetListOfValidCurrencies()
        {
            var currencies = _converter.GetCurrencies().Select(c => new { CurrencyCode = c.CurrencyCode, CurrencyName = c.CurrencyName });
            return Ok(currencies);
        }
    }
}
