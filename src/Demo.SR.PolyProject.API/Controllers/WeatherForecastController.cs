using Demo.SR.PolyProject.API.Middlewares;
using Demo.SR.PolyProject.API.Services;
using Demo.SR.PolyProject.API.Utilities;
using Demo.SR.PolyProject.API.Utilitities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Demo.SR.PolyProject.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
       

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IWeatherService _weatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<string> Get(string cityName="Kolkata")
        {
            Trace.TraceMessage(_logger, "Logging from controller");

            return await _weatherService.GetWeatherDetailsByCityName(cityName);
        }
    }
}
