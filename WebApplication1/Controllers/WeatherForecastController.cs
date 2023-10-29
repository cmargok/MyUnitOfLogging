using K.Loggger.Client.Logger;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{



    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMyLogger _log;

    public WeatherForecastController(IMyLogger log)
    {
        _log = log;
    }

    private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

      
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _log.LoggingInformation("Logueando Informacion");
            _log.LoggingWarning("Logueando Un Warning");

            try
            {
                throw new Exception("siks miks");

            }
            catch (Exception e)
            {
                _log.LoggingError(e,"Soy UN errorrsote");
                _log.LoggingError(e, "Soy UN errorrsote");
                _log.LoggingError(e, "Soy UN asa");
                _log.LoggingError(e, "Soy UN errorrsote");
                _log.LoggingError(e, "Soy UN errorrsote");

                _log.LoggingError(e, "Soy UN errorrsote");
                _log.LoggingError(e, "Soy UN errorrsote");

                _log.LoggingError(e, "Soy UN errorrsote");
                _log.LoggingError(e, "Soy UN errorrsote");
                _log.LoggingError(e, "Soy UN errorrsote");
             

            }
            

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}